namespace LibraryManagementApp.Data
{
    public class Wishlist : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Book Book { get; set; } = null!;
        public DateTime AddedAt { get; set; }

    }
}
