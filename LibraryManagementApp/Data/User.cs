using LibraryManagementApp.Core.Enums;

namespace LibraryManagementApp.Data
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public UserRole UserRole { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public virtual Reader? Reader { get; set; } 
        public virtual Librarian? Librarian { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; } = new HashSet<Wishlist>();
    }
}
