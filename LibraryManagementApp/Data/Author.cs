namespace LibraryManagementApp.Data
{
    public class Author : BaseEntity
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public virtual ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}
