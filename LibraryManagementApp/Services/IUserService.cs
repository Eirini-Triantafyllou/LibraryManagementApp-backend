using LibraryManagementApp.Core.Enums;
using LibraryManagementApp.Core.Filters;
using LibraryManagementApp.Data;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Services
{
    public interface IUserService
    {
        Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials);
        Task<UserReadOnlyDTO?> GetUserByUsernameAsync(string username);
        Task<PaginatedResult<UserReadOnlyDTO>> GetPaginatedUsersFilteredAsync(int pageNumber, int pageSize, UserFiltersDTO userFiltersDTO);
        Task<UserReadOnlyDTO> GetUserByIdAsync(int id);
        Task<UserReaderReadOnlyDTO?> GetUserReaderByUsernameAsync(string username);
        string CreateUserToken(int userId, string username, string email,
                          UserRole userRole, string secretKey);
        Task<UserReadOnlyDTO?> FindUserByUsernameAsync(string username);

        Task<UserReadOnlyDTO?> UpdateUserAsync(int userId, UpdateUserReaderDTO dto);
        Task <bool> DeleteUserAsync(int userId);
    }
}
