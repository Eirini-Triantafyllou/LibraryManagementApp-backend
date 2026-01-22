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

        public async Task<bool> BookExistsAsync(int bookId)
        {
            return await context.Books
                .AnyAsync(b => b.Id == bookId);
        }

        public async Task<Book?> GetBookAsync(int bookId)
        {
            return await context.Books
                .Where(b => b.Id == bookId)
                .Include(b => b.Author)
                .FirstOrDefaultAsync();
        }

        public async Task<Book?> UpdateAsync(int id, Book book)
        {
            var existingBook = await context.Books.FindAsync(id);
            if (existingBook == null)
            {
                return null;
            }

            context.Books.Attach(book);
            context.Entry(book).State = EntityState.Modified;    // Marks book entity as modified

            return existingBook;
        }

        public async Task<bool> IsISBNExistsAsync(string isbn)
        {
            return await context.Books
                .AnyAsync(b => b.ISBN == isbn && !b.IsDeleted);
        }
    }
}
