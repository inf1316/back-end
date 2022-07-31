using System;

namespace QvaCar.Application.Features.Exceptions
{
    public class UserAlreadyExistDomainException : Exception
    {
        public UserAlreadyExistDomainException() { }
        public UserAlreadyExistDomainException(string message) : base(message) { }
        public UserAlreadyExistDomainException(string message, Exception inner) : base(message, inner) { }
    }
}
