namespace ApiApplication.Exceptions
{
    public class ApplicationException : BaseException
    {
        public ApplicationException(string message) : base(message)
        {
            Code = 500;
        }

        public ApplicationException(string type, int code, string message) : this(message)
        {
            Type = type;
            Code = code;
        }
    }
}
