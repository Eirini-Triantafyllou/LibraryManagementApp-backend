using AutoMapper;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementApp.Controllers
{
    public class WishlistController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<WishlistController> logger;
        public WishlistController(IApplicationService applicationService, IConfiguration configuration, ILogger<WishlistController> logger) :
            base(applicationService)
        {
            this.configuration = configuration;
            this.logger = logger;
        }


        // user ID από JWT
        private int GetCurrentUserId()
        {
            if (!User.Identity!.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? User.FindFirst("sub")?.Value
                   ?? User.FindFirst("userId")?.Value
                   ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user authentication.");
            }

            return userId;
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<IEnumerable<WishlistItemDTO>>> GetUserWishlist()
        {
            var userId = GetCurrentUserId();
            var wishlistItems = await applicationService.WishlistService.GetUserWishlistAsync(userId);
            return Ok(wishlistItems);
        }

        [HttpGet("{bookId}")]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<bool>> CheckIfInWishlist(int bookId)
        {
            var userId = GetCurrentUserId();
            var isInWishlist = await applicationService.WishlistService.IsBookInWishlistAsync(userId, bookId);
            return Ok(isInWishlist);
        }

        [HttpPost("{bookId}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> AddToWishlist(int bookId)
        {
            var userId = GetCurrentUserId();
            await applicationService.WishlistService.AddToWishlistAsync(userId, bookId);
            return Ok(new
            {
                success = true,
                message = "Book added to wishlist successfully."
            });
        }

        [HttpDelete("{bookId}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> RemoveFromWishlist(int bookId)
        {
            var userId = GetCurrentUserId();
            await applicationService.WishlistService.RemoveFromWishlistAsync(userId, bookId);
            return Ok(new
            {
                success = true,
                message = "Book removed from wishlist successfully."
            });
        }
    }
}
