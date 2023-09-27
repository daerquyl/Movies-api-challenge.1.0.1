namespace ApiApplication.Exceptions
{
    public class UserInputValidationException : BaseException
    {
        public UserInputValidationException(string type, string message) : base(message)
        {
            Code = 400;
            Type = type;
        }
    }
}
