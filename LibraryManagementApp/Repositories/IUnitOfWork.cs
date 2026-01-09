namespace LibraryManagementApp.Repositories
{
    public interface IUnitOfWork
    {

        UserRepository UserRepository { get; }
        AuthorRepository AuthorRepository { get; }
        BookRepository BookRepository { get; }
        ReaderRepository ReaderRepository { get; }
        LibrarianRepository LibrarianRepository { get; }
        WishlistRepository WishlistRepository { get; }

        Task<bool> SaveAsync();
    }
}
