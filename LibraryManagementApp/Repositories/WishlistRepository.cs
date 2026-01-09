using Microsoft.EntityFrameworkCore;
using LibraryManagementApp.Data;
using LibraryManagementApp.DTO;
using System.Linq.Expressions;

namespace LibraryManagementApp.Repositories
{
    public class WishlistRepository : BaseRepository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(LibraryAppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsBookInWishlistAsync(int userId, int bookId)
        {
            return await context.Wishlists
                .AnyAsync(w => w.UserId == userId && w.BookId == bookId);
        }

        public async Task AddToWishlistAsync(int userId, int bookId)
        {
            var wishlistItem = new Wishlist
            {
                UserId = userId,
                BookId = bookId,
                AddedAt = DateTime.UtcNow
            };
            await AddAsync(wishlistItem);
        }

        public async Task<IEnumerable<WishlistItemDTO>> GetUserWishlistAsync(int userId)
        {
            return await context.Wishlists
                .Where(w => w.UserId == userId)
                .Include(w => w.Book)
                    .ThenInclude(b => b.Author)
                .Select(w => new WishlistItemDTO
                {
                    Id = w.Id,
                    BookId = w.BookId,
                    Title = w.Book.Title,
                    ISBN = w.Book.ISBN,
                    AuthorName = w.Book.Author != null 
                            ? $"{w.Book.Author.Firstname} {w.Book.Author.Lastname}" 
                            : "Unknown",
                    AddedAt = w.AddedAt
                })
                .ToListAsync();
        }
        public async Task RemoveFromWishlistAsync(int userId, int bookId)
        {
            var wishlistItem = await context.Wishlists
               .FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == bookId);

            if (wishlistItem == null) return;

            await DeleteAsync(wishlistItem!.Id);
        }
    }
}
