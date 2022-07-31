using System;

namespace QvaCar.Application.Exceptions
{
    public class UserNotFoundDomainException : Exception
    {
        public UserNotFoundDomainException() { }
        public UserNotFoundDomainException(string message) : base(message) { }
        public UserNotFoundDomainException(string message, Exception inner) : base(message, inner) { }
    }
}
