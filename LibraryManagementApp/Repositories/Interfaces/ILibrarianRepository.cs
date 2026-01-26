using LibraryManagementApp.Data;
using LibraryManagementApp.Models;
using System.Linq.Expressions;

namespace LibraryManagementApp.Repositories.Interfaces
{
    public interface ILibrarianRepository
    {
        Task <Librarian?> GetLibrarianWithDetailsAsync(int librarianId);
        Task <User?> GetUserLibrarianByUsernameAsync(string username);

        Task<PaginatedResult<User>> GetPaginatedUsersLibrariansAsync(int pageNumber, int pageSize);
        Task<PaginatedResult<User>> GetPaginatedLibrariansFilteredAsync(int pageNumber, int pageSize,
             List<Expression<Func<User, bool>>> predicates);
    }
}
