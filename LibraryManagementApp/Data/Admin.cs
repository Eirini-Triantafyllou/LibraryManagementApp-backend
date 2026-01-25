namespace LibraryManagementApp.Data
{
    public class Admin : BaseEntity
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
