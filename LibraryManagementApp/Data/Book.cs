namespace LibraryManagementApp.Data
{
    public class Book : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public virtual Author? Author { get; set; } = null!;
        public int AuthorId { get; set; }
        public DateTime PublishedDate { get; set; }
        public int CopiesAvailable { get; set; }
        public virtual ICollection<Reader> Readers { get; set; } = new HashSet<Reader>();

    }
}
