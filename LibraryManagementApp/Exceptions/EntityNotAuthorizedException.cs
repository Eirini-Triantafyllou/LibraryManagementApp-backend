namespace LibraryManagementApp.Exceptions
{
    public class EntityNotAuthorizedException : AppException
    {
        private static readonly string DEFAULT_CODE = "entity_not_authorized";
        public EntityNotAuthorizedException(string code, string message)
            : base(code + DEFAULT_CODE, message)
        {
        }
    }
}
    

