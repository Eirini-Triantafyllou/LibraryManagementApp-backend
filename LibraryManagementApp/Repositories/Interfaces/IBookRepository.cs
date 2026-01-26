using LibraryManagementApp.Data;

namespace LibraryManagementApp.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Reader>> GetBookReadersAsync(int bookId);
        Task<bool> BookExistsAsync(int bookId);
        Task<bool> IsISBNExistsAsync(string isbn);
    }
}
