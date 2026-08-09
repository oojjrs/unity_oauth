using System;
using System.Collections.Generic;

namespace oojjrs.oauth
{
    public sealed class AuthenticationServiceException : AuthenticationRequestFailedException
    {
        public IReadOnlyList<AuthenticationNotification> Notifications { get; }

        internal AuthenticationServiceException(int errorCode, string message, Exception innerException,
            IReadOnlyList<AuthenticationNotification> notifications)
            : base(errorCode, message, innerException)
        {
            Notifications = notifications ?? Array.Empty<AuthenticationNotification>();
        }
    }
}
