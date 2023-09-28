using ApiApplication.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Domain.Exceptions
{
    public abstract class DomainException : BaseException
    {
        public DomainException(string type, int code, string message) : base(message)
        {
            Code = code;
            Type = type;
        }
    }
}