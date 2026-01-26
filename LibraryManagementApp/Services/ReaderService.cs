using AutoMapper;
using LibraryManagementApp.Core.Enums;
using LibraryManagementApp.Data;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Exceptions;
using LibraryManagementApp.Repositories.Interfaces;
using LibraryManagementApp.Security;
using Serilog;

namespace LibraryManagementApp.Services
{
    public class ReaderService : IReaderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<ReaderService> logger = new LoggerFactory().AddSerilog().CreateLogger<ReaderService>();
        public ReaderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        //public async Task<UserReadOnlyDTO?> SignUpUserAsync(UserSignupDTO request)
        //{
            
        //    try
        //    {
        //        var existingUser = await unitOfWork.UserRepository.GetUserAsync(request.Username!, request.Email!);

        //        if (existingUser != null)
        //        {
        //            throw new EntityAlreadyExistsException("User", "User already exists");
        //        }

        //        var user = new User
        //        {
        //            Username = request.Username!,
        //            Email = request.Email!,
        //            Password = EncryptionUtil.Encrypt(request.Password!),
        //            Firstname = request.Firstname!,
        //            Lastname = request.Lastname!,
        //            UserRole = request.UserRole!,
        //            PhoneNumber = request.PhoneNumber!,
        //            Address = request.Address!
        //        };

        //        await unitOfWork.UserRepository.AddAsync(user);
        //        await unitOfWork.SaveAsync();

        //        switch (request.UserRole)
        //        {
        //            case UserRole.Reader:
        //                var reader = new Reader
        //                {
        //                    UserId = user.Id,
        //                    Firstname = user.Firstname,  
        //                    Lastname = user.Lastname,   
        //                    Email = user.Email,          
        //                    PhoneNumber = user.PhoneNumber, 
        //                    Address = user.Address,      
        //                    MembershipDate = DateTime.UtcNow, 
        //                    IsActive = true,             
        //                    BooksBorrowedCount = 0      
        //                };
        //                await unitOfWork.ReaderRepository.AddAsync(reader);
        //                break;
                        
        //             case UserRole.Librarian:
        //                var librarian = new Librarian
        //                {
        //                    UserId = user.Id,
        //                    Firstname = user.Firstname,
        //                    Lastname = user.Lastname,
        //                    Email = user.Email,
        //                    PhoneNumber = user.PhoneNumber,
        //                    Address = user.Address,
        //                    HireDate = DateTime.UtcNow,  
        //                    IsActive = true              
        //                };
        //                await unitOfWork.LibrarianRepository.AddAsync(librarian);
        //                break;

        //              case UserRole.Admin:
        //                var admin = new Admin
        //                {
        //                    UserId = user.Id,
        //                    Firstname = user.Firstname,
        //                    Lastname = user.Lastname,
        //                    Email = user.Email,
        //                    PhoneNumber = user.PhoneNumber,
        //                    Address = user.Address
        //                };
        //                await unitOfWork.AdminRepository.AddAsync(admin);
        //                break;

        //               default:
        //                throw new InvalidRegistrationException("Invalid user role specified during registration.");
  
        //        }

        //        await unitOfWork.SaveAsync();
        //        return mapper.Map<UserReadOnlyDTO>(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.InnerException != null)
        //            Console.WriteLine($"INNER: {ex.InnerException.Message}");
        //        throw;
        //    }

        //}

    }
}