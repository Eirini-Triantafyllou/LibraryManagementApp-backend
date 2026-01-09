using LibraryManagementApp.DTO;

namespace LibraryManagementApp.Repositories
{
    public interface IWishlistRepository
    {
        Task<IEnumerable<WishlistItemDTO>> GetUserWishlistAsync(int userId);
        Task<bool> IsBookInWishlistAsync(int userId, int bookId);
        Task AddToWishlistAsync(int userId, int bookId);
        Task RemoveFromWishlistAsync(int userId, int bookId);

    }
}
