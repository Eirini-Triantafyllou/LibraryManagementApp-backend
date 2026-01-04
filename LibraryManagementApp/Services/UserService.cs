using AutoMapper;
using Azure.Core;
using LibraryManagementApp.Core.Enums;
using LibraryManagementApp.Core.Filters;
using LibraryManagementApp.Data;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Exceptions;
using LibraryManagementApp.Models;
using LibraryManagementApp.Repositories;
using LibraryManagementApp.Security;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace LibraryManagementApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<UserService> logger =
            new LoggerFactory().AddSerilog().CreateLogger<UserService>();

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<PaginatedResult<UserReadOnlyDTO>> GetPaginatedUsersFilteredAsync(int pageNumber, int pageSize, UserFiltersDTO userFiltersDTO)
        {
            List<User> users = [];
            List<Expression<Func<User, bool>>> predicates = [];

            if (!string.IsNullOrEmpty(userFiltersDTO.Username))
            {
                predicates.Add(u => u.Username == userFiltersDTO.Username);
            }
            if (!string.IsNullOrEmpty(userFiltersDTO.Email))
            {
                predicates.Add(u => u.Email == userFiltersDTO.Email);
            }
            if (!string.IsNullOrEmpty(userFiltersDTO.UserRole))
            {
                predicates.Add(u => u.UserRole.ToString() == userFiltersDTO.UserRole);
            }

            var result = await unitOfWork.UserRepository.GetUsersAsync(pageNumber, pageSize, predicates);

            var dtoResult = new PaginatedResult<UserReadOnlyDTO>()
            {
                Data = result.Data.Select(u => new UserReadOnlyDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    UserRole = u.UserRole.ToString()!
                }).ToList(),
                TotalRecords = result.TotalRecords,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
            logger.LogInformation("Retrieved {Count} users", dtoResult.Data.Count);
            return dtoResult;
        }

        public async Task<UserReadOnlyDTO?> GetUserByUsernameAsync(string username)
        {
            try
            {
                User? user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    throw new EntityNotFoundException("User", "User with username: " + username + " not found.");
                }

                logger.LogInformation("User with username: {Username} not found.", username);
                return new UserReadOnlyDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    UserRole = user.UserRole.ToString()!
                };
            }
            catch (EntityNotFoundException e)
            {
                logger.LogError("Error retrieving user with username: {Username}. {Message}", username, e.Message);
                throw;
            }
        }

        public async Task<UserReaderReadOnlyDTO?> GetUserReaderByUsernameAsync(string username)
        {
            UserReaderReadOnlyDTO? userReaderReadOnlyDTO = null;
            try
            {
                userReaderReadOnlyDTO = await unitOfWork.UserRepository.GetUserReaderAsync(username);
                if (userReaderReadOnlyDTO == null)
                {
                    throw new EntityNotFoundException("User", "User with username: " + username + " not found.");
                }
                logger.LogInformation("User with username: {Username} found.", username);
                return userReaderReadOnlyDTO;
            }
            catch (EntityNotFoundException e)
            {
                logger.LogError("Error retrieving user with username: {Username}. {Message}", username, e.Message);
                throw;
            }
        }

        public async Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(credentials.Email) ||
        string.IsNullOrWhiteSpace(credentials.Password))
            {
                logger.LogWarning("Login attempt with empty email or password");
                return null;
            }

            try
            {
                var user = await unitOfWork.UserRepository.GetByEmailAsync(credentials.Email);
                if (user == null)
                {
                    logger.LogWarning("User not found for email: {Email}", credentials.Email);
                    return null;

                }

                // Επαλήθευση password 
                bool isValid = EncryptionUtil.IsValidPassword(credentials.Password, user.Password);

                logger.LogInformation("Password validation for {Email}: {IsValid}",
                    credentials.Email, isValid);

                if (!isValid)
                {
                    logger.LogWarning("Invalid password for email: {Email}", credentials.Email);
                    return null;
                }

                // Επιτυχημένο login
                logger.LogInformation("User {UserId} logged in successfully", user.Id);
                return user;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error during login verification for email: {Email}",
                credentials.Email);
                return null;
            }  
        }


        public async Task<UserReadOnlyDTO> GetUserByIdAsync(int id)
        {
            User? user = null;
            try
            {
                user = await unitOfWork.UserRepository.GetAsync(id);
                if (user == null)
                {
                    throw new EntityNotFoundException("User", "User with id: " + id + " not found.");
                }
                logger.LogInformation("User with id: {Id} found.", id);
                return mapper.Map<UserReadOnlyDTO>(user);
            }
            catch (EntityNotFoundException e)
            {
                logger.LogError("Error retrieving user with id: {Id}. {Message}", id, e.Message);
                throw;
            }
        }

        public string CreateUserToken(int userId, string username, string email, UserRole userRole,
            string appSecurityKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSecurityKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsInfo = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, userRole.ToString())
            };

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: claimsInfo,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: signingCredentials
            );

            // Serialize the token to a string
            var userToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return userToken;

        }

        public async Task<UserReadOnlyDTO?> FindUserByUsernameAsync(string username)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            return user == null ? null : mapper.Map<UserReadOnlyDTO>(user);
        }

        public async Task<UserReadOnlyDTO?> UpdateUserAsync(int userId, UpdateUserReaderDTO dto)
        {
            // 1. Εύρεση του χρήστη
            var user = await unitOfWork.UserRepository.GetAsync(userId)
                ?? throw new EntityNotFoundException("User", $"User with ID {userId} not found");

            // 2. Έλεγχος μοναδικότητας email
            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
            {
                var emailExists = await unitOfWork.UserRepository
                    .ExistsByEmailAsync(dto.Email);

                if (emailExists)
                {
                    throw new EntityAlreadyExistsException("User",
                        $"Email {dto.Email} is already in use.");
                }
            }

            // 3. Mapping και αποθήκευση
            mapper.Map(dto, user);
            user.ModifiedAt = DateTime.UtcNow;

            await unitOfWork.UserRepository.UpdateAsync(user);
            await unitOfWork.SaveAsync();

            logger.LogInformation("User updated: {UserId}", user.Id);

            return mapper.Map<UserReadOnlyDTO>(user);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await unitOfWork.UserRepository.GetAsync(userId);
            if (user == null)
            {
                logger.LogWarning("User with ID {UserId} not found for deletion.", userId);
                return false;
            }
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            user.ModifiedAt = DateTime.UtcNow;
            await unitOfWork.UserRepository.UpdateAsync(user);
            await unitOfWork.SaveAsync();
            logger.LogInformation("User with ID {UserId} marked as deleted.", userId);
            return true;
        }
    }
}