namespace LibraryManagementApp.Exceptions
{
    public class InvalidArgumentException : AppException
    {
        private static readonly string DEFAULT_CODE = "invalid_argument";
        public InvalidArgumentException(string code, string message)
            : base(code + DEFAULT_CODE, message)
        {
        }
    }
}
