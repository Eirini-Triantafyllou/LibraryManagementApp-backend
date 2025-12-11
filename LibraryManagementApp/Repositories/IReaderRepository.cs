using LibraryManagementApp.Models;
using LibraryManagementApp.Data;
using System.Linq.Expressions;

namespace LibraryManagementApp.Repositories
{
    public interface IReaderRepository
    {
        Task<List<Book>> GetBorrowedBooksAsync(int readerId);
        Task<Reader?> GetReaderByFullNameAsync(string fullName);
        Task<PaginatedResult<User>> GetPaginatedUsersReadersAsync(int pageNumber, int pageSize,
            List<Expression<Func<User, bool>>> predicates);

    }
}
