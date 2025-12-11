using LibraryManagementApp.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryAppDbContext context) : base(context)
        {
        }

        public async Task<List<Reader>> GetBookReadersAsync(int bookId)
        {
            return await context.Books
                .Where(b => b.Id == bookId)
                .SelectMany(b => b.Readers)
                .ToListAsync();
        }
    }
}
