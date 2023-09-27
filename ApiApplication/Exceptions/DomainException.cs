using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Exceptions
{
    public abstract class DomainException: BaseException
    {
        public DomainException(string type, int code, string message): base(message)
        {
            Code = code;
            Type = type;
        }
    }
}