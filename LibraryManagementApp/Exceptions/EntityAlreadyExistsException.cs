namespace LibraryManagementApp.Exceptions
{
    public class EntityAlreadyExistsException : AppException
    {
        private static readonly string DEFAULT_CODE = "entity_already_exists";

        public EntityAlreadyExistsException(string code, string message) : base(code, message)
        {
        }
    }
}
