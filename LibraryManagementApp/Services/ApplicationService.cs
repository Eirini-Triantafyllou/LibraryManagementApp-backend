using AutoMapper;
using LibraryManagementApp.Repositories;

namespace LibraryManagementApp.Services
{
    public class ApplicationService : IApplicationService
    {
        public IUserService UserService { get; }
        public IReaderService ReaderService { get; }
        public ILibrarianService LibrarianService { get; }

        public ApplicationService(
            IUserService userService,
            IReaderService readerService,
            ILibrarianService librarianService)
        {
            UserService = userService;
            ReaderService = readerService;
            LibrarianService = librarianService;
        }
    }
}
