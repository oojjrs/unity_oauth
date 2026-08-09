using System;

namespace oojjrs.oauth
{
    public class AuthenticationFlowException : Exception
    {
        internal AuthenticationFlowException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
