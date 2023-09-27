namespace ApiApplication.Exceptions
{
    public class Problem
    {
        public string Message;
        public int Code;
        public string Type;

        public Problem(string message, int code, string type)
        {
            this.Message = message;
            this.Code = code;
            this.Type = type;
        }
    }
}
