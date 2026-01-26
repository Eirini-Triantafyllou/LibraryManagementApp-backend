using AutoMapper;
using LibraryManagementApp.Repositories;
using LibraryManagementApp.Services.Interfaces;

namespace LibraryManagementApp.Services
{
    public class ApplicationService : IApplicationService
    {
        public IUserService UserService { get; }
        public IReaderService ReaderService { get; }
        public ILibrarianService LibrarianService { get; }
        public IBookService BookService { get; }
        public IWishlistService WishlistService { get; }

        public ApplicationService(
            IUserService userService,
            IReaderService readerService,
            ILibrarianService librarianService,
            IBookService bookService,
            IWishlistService wishlistService)
        {
            UserService = userService;
            ReaderService = readerService;
            LibrarianService = librarianService;
            BookService = bookService;
            WishlistService = wishlistService;
        }
    }
}
