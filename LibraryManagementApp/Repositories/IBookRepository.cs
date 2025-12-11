using LibraryManagementApp.Data;

namespace LibraryManagementApp.Repositories
{
    public interface IBookRepository
    {
        Task<List<Reader>> GetBookReadersAsync(int bookId);
    }
}
