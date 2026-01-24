namespace LibraryManagementApp.DTO
{
    public record WishlistItemDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string AuthorFullName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime AddedAt { get; set; }

    }
}
