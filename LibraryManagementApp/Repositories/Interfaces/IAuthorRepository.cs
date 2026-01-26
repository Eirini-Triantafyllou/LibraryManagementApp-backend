using LibraryManagementApp.Data;
using LibraryManagementApp.Models;
using System.Linq.Expressions;

namespace LibraryManagementApp.Repositories.Interfaces
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        Task<List<Book>> GetAuthorBooksAsync(int authorId);
        Task<Author?> GetByNameAsync(string authorName);
        Task<PaginatedResult<Book>> GetPaginatedBooksByAuthorNameAsync(string authorName, int pageNumber, int pageSize);
        Task<Author?> GetAuthorWithDetailsAsync(int authorId);
        Task<PaginatedResult<Author>> GetPaginatedAuthorsFilteredAsync(int pageNumber, int pageSize,
             List<Expression<Func<Author, bool>>> predicates);

    }
}
