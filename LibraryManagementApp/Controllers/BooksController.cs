using AutoMapper;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Models;
using LibraryManagementApp.Services;
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

                // Αν δεν βρέθηκαν αποτελέσματα, επιστρέφουμε 200 με άδεια λίστα
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
    }
}
