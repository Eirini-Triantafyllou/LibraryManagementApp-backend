using AutoMapper;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Exceptions;
using LibraryManagementApp.Models;
using LibraryManagementApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApp.Controllers
{
    public class BooksController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly ILogger<BooksController> logger;
        public BooksController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper, ILogger<BooksController> logger) :
            base(applicationService)
        {
            this.configuration = configuration;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<BookByAuthorDTO>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<ActionResult<PaginatedResult<BookByAuthorDTO>>> SearchBooksByAuthorName(
            [FromQuery] string authorName,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                logger.LogInformation(
                    "Αναζήτηση βιβλίων για συγγραφέα: {AuthorName}", authorName);

                // validation
                if (string.IsNullOrWhiteSpace(authorName))
                {
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Μη έγκυρη παράμετρος",
                        Detail = "Το όνομα συγγραφέα δεν μπορεί να είναι κενό.",
                        Status = StatusCodes.Status400BadRequest,
                        Instance = HttpContext.Request.Path
                    });
                }

                // Validation για pagination
                if (pageNumber < 1)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Μη έγκυρη παράμετρος",
                        Detail = "Ο αριθμός σελίδας πρέπει να είναι μεγαλύτερος από 0.",
                        Status = StatusCodes.Status400BadRequest
                    });
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Μη έγκυρη παράμετρος",
                        Detail = "Το μέγεθος σελίδας πρέπει να είναι μεταξύ 1 και 100.",
                        Status = StatusCodes.Status400BadRequest
                    });
                }

                var result = await applicationService.BookService
                    .SearchBooksByAuthorNameAsync(authorName, pageNumber, pageSize);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "Μη έγκυρη παράμετρος: {AuthorName}", authorName);
                return BadRequest(new ProblemDetails
                {
                    Title = "Μη έγκυρη παράμετρος",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Σφάλμα κατά την αναζήτηση βιβλίων για συγγραφέα: {AuthorName}",
                    authorName);

                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Εσωτερικό σφάλμα",
                    Detail = "Προέκυψε σφάλμα κατά την αναζήτηση βιβλίων.",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet("{bookId}")]
        [Authorize(Roles = "Librarian, Admin")]
        public async Task<ActionResult<BookByAuthorDTO>> GetBookByIdAsync(int bookId)
        {
            var bookDTO = await applicationService.BookService.GetBookByIdAsync(bookId);
            return Ok(bookDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Librarian, Admin")]
        [ProducesResponseType(typeof(BookByAuthorDTO), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<ActionResult<BookByAuthorDTO>> CreateBookAsync(CreateBookDTO dto)
        {
            try
            {
                var result = await applicationService.BookService.CreateBookAsync(dto);

                logger.LogInformation($"Book created successfully with ID: {result.Id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "FULL ERROR creating book");

                return StatusCode(500, new
                {
                    code = 500,
                    message = $"Error creating book with ISBN {dto.ISBN}",
                    detailed = ex.Message,
                    innerException = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpPut ("{bookId}")]
        [Authorize(Roles = "Librarian, Admin")]
        public async Task<ActionResult<BookByAuthorDTO>> UpdateBookAsync (int bookId, UpdateBookDTO dto)
        {
            var updatedBookDTO = await applicationService.BookService.UpdateBookAsync(bookId, dto);
            return Ok (updatedBookDTO);
        }


        [HttpDelete("{bookId}")]
        [Authorize(Roles = "Librarian, Admin")]

        public async Task<ActionResult> DeleteBooks(int bookId)
        {
            await applicationService.BookService.DeleteBookAsync(bookId);
            return Ok(new
            {
                success = true,
                message = "Book removed successfully."
            });
        }
    }
}
