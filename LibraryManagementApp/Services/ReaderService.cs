using AutoMapper;
using LibraryManagementApp.Core.Enums;
using LibraryManagementApp.Data;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Exceptions;
using LibraryManagementApp.Repositories;
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

        public async Task<UserReadOnlyDTO?> SignUpUserAsync(UserSignupDTO request)
        {
            
            try
            {
                // ΜΟΝΟ USER (ΧΩΡΙΣ READER)
                var user = new User
                {
                    Username = request.Username!,
                    Email = request.Email!,
                    Password = EncryptionUtil.Encrypt(request.Password!),
                    Firstname = request.Firstname!,
                    Lastname = request.Lastname!,
                    //UserRole = UserRole.Reader // Ρητά ορισμένο
                    UserRole = request.UserRole!,
                    PhoneNumber = request.PhoneNumber!,
                    Address = request.Address!
                };

                await unitOfWork.UserRepository.AddAsync(user);
                await unitOfWork.SaveAsync();

                return mapper.Map<UserReadOnlyDTO>(user);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    Console.WriteLine($"INNER: {ex.InnerException.Message}");
                throw;
            }

        }

        private User ExtractUser(UserSignupDTO signupDTO)
        {
            return new User()
            {
                Username = signupDTO.Username!,
                Password = signupDTO.Password!,
                Email = signupDTO.Email!,
                Firstname = signupDTO.Firstname!,
                Lastname = signupDTO.Lastname!,
                //UserRole = UserRole.Reader
                UserRole = signupDTO.UserRole!
            };
        }

        private Reader ExtractReader(UserSignupDTO signupDTO)
        {
            return new Reader()
            {
                Address = signupDTO!.Address!,
                PhoneNumber = signupDTO.PhoneNumber!, 
            };
        }

        

    }
}