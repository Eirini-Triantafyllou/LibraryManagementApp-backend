using LibraryManagementApp.DTO;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Services
{
    public interface IBookService
    {
        Task<PaginatedResult<BookByAuthorDTO>> SearchBooksByAuthorNameAsync(string authorName, int pageNumber, int pageSize);
    }
}
