namespace LibraryManagementApp.Exceptions
{
    public class EntityNotFoundException : AppException
    {
        private static readonly string DEFAULT_CODE = "entity_not_found";
        public EntityNotFoundException(string code, string message)
            : base(code + DEFAULT_CODE, message)
        {
        }
    }
    
}
