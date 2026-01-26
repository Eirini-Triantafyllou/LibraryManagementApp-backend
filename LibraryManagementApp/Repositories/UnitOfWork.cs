
using LibraryManagementApp.Data;
using LibraryManagementApp.Repositories.Interfaces;

namespace LibraryManagementApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryAppDbContext context;
        public UnitOfWork(LibraryAppDbContext context)
        {
            this.context = context;
        }

        public UserRepository UserRepository => new(context);

        public AuthorRepository AuthorRepository => new(context);

        public BookRepository BookRepository => new(context);

        public ReaderRepository ReaderRepository => new(context);

        public LibrarianRepository LibrarianRepository => new(context);
        public AdminRepository AdminRepository => new(context);

        public WishlistRepository WishlistRepository => new(context);

        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
