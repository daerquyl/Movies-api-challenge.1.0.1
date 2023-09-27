using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Exceptions
{
    public abstract class BaseException: System.Exception
    {
        public int Code { get; set; } = 500;
        public string Type { get; set; }

        public BaseException(string message): base(message)
        {
        }

        public Problem ToProblem()
        {
            return new Problem(Message, Code, Type);
        }
    }
}