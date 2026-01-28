using AutoMapper;
using LibraryManagementApp.Data;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Exceptions;
using LibraryManagementApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;


namespace LibraryManagementApp.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly ILogger<UsersController> logger;
        public UsersController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper, ILogger<UsersController> logger) :
            base(applicationService)
        {
            this.configuration = configuration;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<UserReadOnlyDTO>> SignUpUser(UserSignupDTO signupDTO)
        {
            UserReadOnlyDTO? readOnlyDTO;
            UserReadOnlyDTO? returnedUserDTO;

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(e => e.Value!.Errors.Any())
                .Select(e => new
                {
                    Field = e.Key,
                    Errors = e.Value!.Errors.Select(er => er.ErrorMessage).ToArray()
                });
                throw new InvalidRegistrationException("Invalid registration data" + errors);
            }

            readOnlyDTO = await applicationService.UserService.FindUserByUsernameAsync(signupDTO.Username!);
            if (readOnlyDTO != null)
            {
                throw new EntityAlreadyExistsException("User", "User: " + readOnlyDTO.Username + " already exists.");
            }
            returnedUserDTO = await applicationService.UserService.SignUpUserAsync(signupDTO);
            return CreatedAtAction(nameof(GetUserById), new { id = returnedUserDTO!.Id }, returnedUserDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadOnlyDTO>> GetUserById(int id)
        {
            UserReadOnlyDTO userReadOnlyDTO = await applicationService.UserService.GetUserByIdAsync(id);
            return Ok(userReadOnlyDTO);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<UserReaderReadOnlyDTO>> GetUserReaderByUsernameAsync(string? username)
        {
            var returnedUserDTO = await applicationService.UserService.GetUserReaderByUsernameAsync(username!);
            return Ok(returnedUserDTO);
        }

        [HttpPost]
        public async Task<ActionResult<JwtTokenDTO>> LoginUserAsync(UserLoginDTO credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            logger.LogInformation("Login attempt for email: {Email}", credentials.Email);

            // Verify user
            var user = await applicationService.UserService.VerifyAndGetUserAsync(credentials);

            if (user == null)
            {
                logger.LogWarning("Login failed for email: {Email}", credentials.Email);
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Create token
            var token = applicationService.UserService.CreateUserToken(
                user.Id,
                user.Username,
                user.Email,
                user.UserRole,
                configuration["Authentication:SecretKey"]!
            );

            // Return response
            var response = new JwtTokenDTO
            {
                Token = token,
                User = new UserReadOnlyDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    UserRole = user.UserRole.ToString()
                },
                ExpiresAt = DateTime.UtcNow.AddHours(1)   // expiration 
            };

            logger.LogInformation("User {UserId} logged in successfully", user.Id);
            return Ok(response);
        }



        [HttpPut("{userId}")]
        public async Task<ActionResult<UserReadOnlyDTO>> UpdateUserAsync(int userId, UpdateUserReaderDTO dto)
        {
            var updatedUserDTO = await applicationService.UserService.UpdateUserAsync(userId, dto);
            return Ok(updatedUserDTO);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUserAsync(int userId)
        {
            bool isDeleted = await applicationService.UserService.DeleteUserAsync(userId);
            if (!isDeleted)
            {
                throw new EntityNotFoundException("User", $"User with ID {userId} not found.");
            }
            return NoContent();
        }
        
    }
}
