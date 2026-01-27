using LibraryManagementApp.Data;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.DTO
{
    public record CreateBookDTO
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters.")]
        public string? Title { get; set; } = null!;

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "AuthorName must be between 5 and 100 characters.")]
        public string? AuthorFullName { get; set; } = null!;

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters.")]
        public string? Description { get; set; } = null!;

        [Required(ErrorMessage = "The {0} field is required.")]
        [RegularExpression(@"^\d{10}(\d{3})?$", ErrorMessage = "Invalid ISBN (10 or 13 digits)")]
        public string? ISBN { get; set; } = null!;

        [Required(ErrorMessage = "The {0} field is required.")]
        [DataType(DataType.Date)]
        public DateTime? PublishedDate { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public int? CopiesAvailable { get; set; }

    }
}
