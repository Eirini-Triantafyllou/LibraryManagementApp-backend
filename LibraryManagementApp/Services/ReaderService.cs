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

        public async Task<UserReadOnlyDTO?> SignUpUserReaderAsync(ReaderSignupDTO request)
        {
            


            // Sign up user with default role reader (τώρα έχω και τους αλλους ρολους)(να το τσεκαρω παλι)
            try
            {
                // ΜΟΝΟ USER, ΧΩΡΙΣ READER
                var user = new User
                {
                    Username = request.Username!,
                    Email = request.Email!,
                    Password = EncryptionUtil.Encrypt(request.Password!),
                    Firstname = request.Firstname!,
                    Lastname = request.Lastname!,
                    //UserRole = UserRole.Reader // Ρητά ορισμένο
                    UserRole = request.UserRole!
                };

                //Console.WriteLine($"DEBUG: Saving user with Role={user.UserRole}");

                await unitOfWork.UserRepository.AddAsync(user);
                await unitOfWork.SaveAsync();

                //Console.WriteLine($"DEBUG: User saved successfully! ID={user.Id}");
                return mapper.Map<UserReadOnlyDTO>(user);
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"ERROR: {ex.GetType().Name}: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"INNER: {ex.InnerException.Message}");
                throw;
            }

            


        }

        private User ExtractUser(ReaderSignupDTO signupDTO)
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

        private Reader ExtractReader(ReaderSignupDTO signupDTO)
        {
            return new Reader()
            {
                Address = signupDTO!.Address!,
                PhoneNumber = signupDTO.PhoneNumber!, 
            };
        }

        

    }
}