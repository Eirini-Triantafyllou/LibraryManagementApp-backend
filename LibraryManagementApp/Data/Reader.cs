namespace LibraryManagementApp.Data
{
    public class Reader : BaseEntity
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime? MembershipDate { get; set; }
        public bool? IsActive { get; set;  } 
        public int? BooksBorrowedCount { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}
