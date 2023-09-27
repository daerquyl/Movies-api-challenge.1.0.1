using System;

namespace ApiApplication.Exceptions
{
    public enum RemoteApiExceptionType { 
        UNAUTHORIZED, 
        SERVICE_UNAVAILABLE,
        UNKNOWN_ERROR,
        SERVER_TIMEOUT
    } 
    public class RemoteApiException: BaseException
    {
        private RemoteApiException(string type, int code, string message)
            :base(message)
        {
            Type = type;
            Code = code;
        }

        public static RemoteApiException Create(RemoteApiExceptionType type, string message="") => type switch
        {
            RemoteApiExceptionType.UNAUTHORIZED => new RemoteApiException("ProvidedAPI.Unauthorized", 401, "Service authorization key required"),
            RemoteApiExceptionType.SERVICE_UNAVAILABLE => new RemoteApiException("ProvidedAPI.ServiceUnavailable", 503, "Service Temporary unavailable"),
            RemoteApiExceptionType.SERVER_TIMEOUT => new RemoteApiException("ProvidedAPI.Timeout", 504, "Server took too long to respond"),
            RemoteApiExceptionType.UNKNOWN_ERROR => new RemoteApiException("ProvidedAPI.ServiceUnavailable", 500, $"Remote Server error: {message}"),
        };
    }
}
