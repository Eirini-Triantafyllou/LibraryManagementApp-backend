using LibraryManagementApp.Models;
using LibraryManagementApp.Data;
using System.Linq.Expressions;
using LibraryManagementApp.DTO;

namespace LibraryManagementApp.Repositories
{
    public interface IUserRepository
    {

        //Task<User?> GetUserAsync(string username, string password);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<PaginatedResult<User>> GetUsersAsync(int pageNumber, int pageSize,
            List<Expression<Func<User, bool>>> predicates);
        Task<UserReaderReadOnlyDTO?> GetUserReaderAsync(string username);
        Task<bool> ExistsByEmailAsync(string email);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> UserExistsAsync(int userId);

    }
}
