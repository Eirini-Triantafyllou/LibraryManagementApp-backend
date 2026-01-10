using AutoMapper;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Models;
using LibraryManagementApp.Repositories;
using Serilog;

namespace LibraryManagementApp.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<BookService> logger = new LoggerFactory().AddSerilog().CreateLogger<BookService>();
        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<PaginatedResult<BookByAuthorDTO>> SearchBooksByAuthorNameAsync(string authorName, int pageNumber, int pageSize)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(authorName))
                {
                    throw new ArgumentException("Author name cannot be null or empty.", nameof(authorName));
                }

                var entityResult = await unitOfWork.AuthorRepository
                    .GetPaginatedBooksByAuthorNameAsync(authorName, pageNumber, pageSize);

                var bookDTOs = mapper.Map<List<BookByAuthorDTO>>(entityResult.Data);
                var result = new PaginatedResult<BookByAuthorDTO>
                {
                    Data = bookDTOs,
                    TotalRecords = entityResult.TotalRecords,
                    PageNumber = entityResult.PageNumber,
                    PageSize = entityResult.PageSize
                };

                return result;
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Σφάλμα κατά την αναζήτηση βιβλίων για τον συγγραφέα: {AuthorName}", authorName);
                throw new ApplicationException(
                    $"Σφάλμα κατά την αναζήτηση βιβλίων για τον συγγραφέα '{authorName}'.", ex);
            }
        }
    }
}
