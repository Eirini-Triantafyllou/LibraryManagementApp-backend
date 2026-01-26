using AutoMapper;
using LibraryManagementApp.Data;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Exceptions;
using LibraryManagementApp.Models;
using LibraryManagementApp.Repositories.Interfaces;
using LibraryManagementApp.Services.Interfaces;
using Serilog;

namespace LibraryManagementApp.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<BookService> logger = new LoggerFactory().AddSerilog().CreateLogger<BookService>();
        private readonly IAuthorRepository authorRepository;
        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.authorRepository = unitOfWork.AuthorRepository;
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

        public async Task<BookByAuthorDTO?> GetBookByIdAsync(int bookId)
        {
            Book? book = null;
            try
            {
                book = await unitOfWork.BookRepository.GetBookAsync(bookId);
                if (book == null)
                {
                    throw new EntityNotFoundException("Book", "Book with id " + bookId + " not found.");
                }
                logger.LogInformation("Book with id {bookId} found.", bookId);
                return mapper.Map<BookByAuthorDTO>(book);
            }
            catch (EntityNotFoundException e)
            {
                logger.LogError("Error retrieving book with id {bookId}. {Message}", bookId, e.Message);
                throw;
            }

        }

        public async Task<BookByAuthorDTO> CreateBookAsync(CreateBookDTO dto)
        {
            try
            {
                if (await unitOfWork.BookRepository.IsISBNExistsAsync(dto.ISBN!))
                {
                    throw new EntityAlreadyExistsException("Book", $"Book with ISBN {dto.ISBN} already exists.");
                }

                var book = mapper.Map<Book>(dto);
                book.InsertedAt = DateTime.UtcNow;
                book.ModifiedAt = DateTime.UtcNow;

                if (!string.IsNullOrWhiteSpace(dto.AuthorFullName))
                {
                    var author = await authorRepository.GetByNameAsync(dto.AuthorFullName!);
                    if (author == null)
                    {
                        author = new Author
                        {
                            AuthorFullName = dto.AuthorFullName!,
                            Biography = null,           
                            DateOfBirth = null,
                            InsertedAt = DateTime.UtcNow,
                            ModifiedAt = DateTime.UtcNow,
                            IsDeleted = false
                        };
                        await authorRepository.AddAsync(author);
                        await unitOfWork.SaveAsync();
                    }
                    book.AuthorId = author.Id;
                }

                await unitOfWork.BookRepository.AddAsync(book);
                await unitOfWork.SaveAsync();

                var createdBook = await unitOfWork.BookRepository.GetBookAsync(book.Id);
                return mapper.Map<BookByAuthorDTO>(createdBook);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating book with ISBN {ISBN}.", dto.ISBN);
                throw new ApplicationException($"Error creating book with ISBN {dto.ISBN}.", ex);
            }

        }

        public async Task<BookByAuthorDTO?> UpdateBookAsync(int bookId, UpdateBookDTO dto)
        {
            var book = await unitOfWork.BookRepository.GetBookAsync(bookId) 
                ?? throw new EntityNotFoundException("Book", $"Book with id {bookId} not found.");

            mapper.Map(dto, book);
            book.ModifiedAt = DateTime.UtcNow;

            await unitOfWork.BookRepository.UpdateAsync(book);
            await unitOfWork.SaveAsync();
            return mapper.Map<BookByAuthorDTO>(book);
        }

        public async Task DeleteBookAsync(int bookId)
        {
            var deletedBook = await unitOfWork.BookRepository.DeleteAsync(bookId);

            if (!deletedBook)
            {
                throw new EntityNotFoundException("Book", $"Book with id {bookId} not found.");
            }
            await unitOfWork.SaveAsync();
        }

        
    }
}
