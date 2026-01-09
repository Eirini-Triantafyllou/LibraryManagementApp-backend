using LibraryManagementApp.Data;
using LibraryManagementApp.Models;
using System.Linq.Expressions;

namespace LibraryManagementApp.Repositories
{
    public interface IAuthorRepository
    {
        Task<List<Book>> GetAuthorBooksAsync(int authorId);

        Task<PaginatedResult<Book>> GetPaginatedBooksByAuthorNameAsync(string authorName, int pageNumber, int pageSize);
        Task<Author?> GetAuthorWithDetailsAsync(int authorId);
        Task<PaginatedResult<Author>> GetPaginatedAuthorsFilteredAsync(int pageNumber, int pageSize,
             List<Expression<Func<Author, bool>>> predicates);

    }
}
