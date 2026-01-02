using LibraryManagementApp.Core.Enums;

namespace LibraryManagementApp.DTO
{
    public record UserReadOnlyDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;

    }
}
