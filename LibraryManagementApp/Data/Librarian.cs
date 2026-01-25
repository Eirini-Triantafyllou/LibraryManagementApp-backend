namespace LibraryManagementApp.Data
{
    public class Librarian : BaseEntity
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime? HireDate { get; set; }
        public bool? IsActive { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Reader> Readers { get; set; } = new HashSet<Reader>();
    }
}
