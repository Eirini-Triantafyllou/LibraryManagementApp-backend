namespace LibraryManagementApp.Exceptions
{
    public class EntityForbiddenException : AppException
    {
        private static readonly string DEFAULT_CODE = "entity_forbidden";
        public EntityForbiddenException(string code, string message)
            : base(code + DEFAULT_CODE, message)
        {
        }
    }
}
