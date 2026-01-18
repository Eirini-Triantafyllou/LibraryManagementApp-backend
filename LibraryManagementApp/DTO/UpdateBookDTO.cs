using LibraryManagementApp.Data;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.DTO
{
    public class UpdateBookDTO
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Book title must be between 5 and 100 characters.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Book title must be between 5 and 500 characters.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "The {0} field is required.")]
        public int CopiesAvailable { get; set; }
    }
}
