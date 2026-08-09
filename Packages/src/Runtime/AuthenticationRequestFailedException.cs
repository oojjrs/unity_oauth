using System;

namespace oojjrs.oauth
{
    public class AuthenticationRequestFailedException : Exception
    {
        public int ErrorCode { get; }

        internal AuthenticationRequestFailedException(int errorCode, string message)
            : this(errorCode, message, null)
        {
        }

        internal AuthenticationRequestFailedException(int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
