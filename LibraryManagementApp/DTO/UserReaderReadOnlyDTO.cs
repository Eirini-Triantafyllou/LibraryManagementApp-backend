namespace LibraryManagementApp.DTO
{
    public record UserReaderReadOnlyDTO
    {
        public int Id { get; set; }
        public string? Username { get; set; } 
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? UserRole { get; set; } 

    }
}
