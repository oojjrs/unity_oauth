using System;

namespace oojjrs.oauth
{
    public class AuthenticationRequestFailedException : AuthenticationFlowException
    {
        public int ErrorCode { get; }

        internal AuthenticationRequestFailedException(int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
