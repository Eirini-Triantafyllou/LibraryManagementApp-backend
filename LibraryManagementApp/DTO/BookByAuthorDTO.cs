using LibraryManagementApp.Data;

namespace LibraryManagementApp.DTO
{
    public record BookByAuthorDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public DateTime PublishedDate { get; set; }
        public int CopiesAvailable { get; set; }
        public string AuthorFullName { get; set; } = null!;
        public int AuthorId { get; set; }

    }
}
