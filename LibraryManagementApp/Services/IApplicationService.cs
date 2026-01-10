namespace LibraryManagementApp.Services
{
    public interface IApplicationService
    {
        IUserService UserService { get; }
        IReaderService ReaderService { get; }
        ILibrarianService LibrarianService { get; }
        IBookService BookService { get; }
        IWishlistService WishlistService { get; }
    }
}
