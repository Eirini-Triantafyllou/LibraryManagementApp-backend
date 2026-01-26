using LibraryManagementApp.DTO;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Services.Interfaces
{
    public interface IBookService
    {
        Task<PaginatedResult<BookByAuthorDTO>> SearchBooksByAuthorNameAsync(string authorName, int pageNumber, int pageSize);

        Task<BookByAuthorDTO?> GetBookByIdAsync(int bookId);

        Task<BookByAuthorDTO> CreateBookAsync(CreateBookDTO dto);

        Task<BookByAuthorDTO?> UpdateBookAsync(int bookId, UpdateBookDTO dto);
        Task DeleteBookAsync(int bookId);


    }
}
