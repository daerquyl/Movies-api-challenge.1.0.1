namespace ApiApplication.Exceptions
{
    public class ApplicationException : BaseException
    {
        public ApplicationException(string message) : base(message)
        {
            Code = 500;
        }
    }
}
