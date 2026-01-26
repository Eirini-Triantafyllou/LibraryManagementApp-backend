using AutoMapper;
using LibraryManagementApp.Data;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Exceptions;
using LibraryManagementApp.Repositories.Interfaces;
using LibraryManagementApp.Services.Interfaces;
using Serilog;

namespace LibraryManagementApp.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<WishlistService> logger = new LoggerFactory().AddSerilog().CreateLogger<WishlistService>();
        public WishlistService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task AddToWishlistAsync(int userId, int bookId)
        {
            try
            {
                var userExists = await unitOfWork.UserRepository.UserExistsAsync(userId);
                if (!userExists)
                {
                    throw new EntityNotFoundException("user_not_found", $"User with ID {userId} not found.");
                }

                var bookExists = await unitOfWork.BookRepository.BookExistsAsync(bookId);
                if (!bookExists)
                {
                    throw new EntityNotFoundException("book_not_found", $"Book with ID {bookId} not found.");
                }

                var alreadyInWishlist = await unitOfWork.WishlistRepository.IsBookInWishlistAsync(userId, bookId);
                if (alreadyInWishlist)
                {
                    throw new EntityAlreadyExistsException("book_already_in_wishlist", $"Book with ID {bookId} is already in the wishlist for user ID {userId}.");
                }

                    await unitOfWork.WishlistRepository.AddToWishlistAsync(userId, bookId);
                    logger.LogInformation("Book {BookId} added to wishlist for user {UserId}",
                    bookId, userId);

                    await unitOfWork.SaveAsync();
            }
                catch (EntityNotFoundException ex)
                {
                    logger.LogWarning(ex, "Validation failed for user {UserId}, book {BookId}", userId, bookId);
                    throw; // Re-throw για Controller
                }
                catch (EntityAlreadyExistsException ex)
                {
                    logger.LogWarning(ex, "User with id {UserId}, already uses book with id {BookId} in his/her wishlist",
                        userId, bookId);
                    throw;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Unexpected error adding to wishlist for user {UserId}, book {BookId}",
                        userId, bookId);
                    throw new Exception("An unexpected error occurred while adding to wishlist.", ex);
                }
        }

        public async Task<IEnumerable<WishlistItemDTO>> GetUserWishlistAsync(int userId)
        {
            try
            {
                var userExists = await unitOfWork.UserRepository.UserExistsAsync(userId);
                if (!userExists)
                {
                    throw new EntityNotFoundException("user_not_found", $"User with ID {userId} not found.");
                }

                var wishlistItems = await unitOfWork.WishlistRepository.GetUserWishlistAsync(userId);

                logger.LogInformation("Retrieved {Count} items from wishlist for user {UserId}",
                        wishlistItems.Count(), userId);

                return wishlistItems;
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogWarning(ex, "User not found");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving wishlist");
                throw new Exception("An error occurred while retrieving wishlist.", ex);
            }
        }

        public async Task<bool> IsBookInWishlistAsync(int userId, int bookId)
        {
            return await unitOfWork.WishlistRepository.IsBookInWishlistAsync(userId, bookId);
               
        }

        public async Task RemoveFromWishlistAsync(int userId, int bookId)
        {
            var userExists = await unitOfWork.UserRepository.UserExistsAsync(userId);
            if (!userExists)
            {
                throw new EntityNotFoundException("user_not_found", $"User with ID {userId} not found.");
            }

            var bookExistsInWishlist = await unitOfWork.WishlistRepository.IsBookInWishlistAsync(userId, bookId);
            if (!bookExistsInWishlist)
            {
                throw new EntityNotFoundException("book_not_found", $"Book with ID {bookId} not found in user's wishlist");
            }

            await unitOfWork.WishlistRepository.RemoveFromWishlistAsync(userId, bookId);
            await unitOfWork.SaveAsync();

            logger.LogInformation("Book {BookId} removed from wishlist for user {UserId}",
                bookId, userId);
        }


    }
}
