namespace ApiApplication.Exceptions
{
    public class SqlException : ApplicationException
    {
        public SqlException(string message) : base(message)
        {
            Type = "ApplicationException.Database";
        }
    }
}
