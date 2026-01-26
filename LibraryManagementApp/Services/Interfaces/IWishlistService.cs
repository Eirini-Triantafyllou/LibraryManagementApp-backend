using LibraryManagementApp.DTO;

namespace LibraryManagementApp.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<bool> IsBookInWishlistAsync(int userId, int bookId);
        Task AddToWishlistAsync(int userId, int bookId);
        Task<IEnumerable<WishlistItemDTO>> GetUserWishlistAsync(int userId);
        Task RemoveFromWishlistAsync(int userId, int bookId);
    }
}
