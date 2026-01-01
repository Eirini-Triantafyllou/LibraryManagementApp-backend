namespace LibraryManagementApp.DTO
{
    public class JwtTokenDTO
    {
        public string? Token { get; set; } = string.Empty;
        public UserReadOnlyDTO? User { get; set; }
    }
}
