using LibraryManagementApp.DTO;

namespace LibraryManagementApp.Services
{
    public interface IReaderService
    {
        Task<UserReadOnlyDTO?> SignUpUserReaderAsync(ReaderSignupDTO request);
    }
}
