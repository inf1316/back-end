using System;

namespace QvaCar.Seedwork.Domain
{
    public class DomainIsInInvalidStateException : DomainException
    {
        public DomainIsInInvalidStateException() { }
        public DomainIsInInvalidStateException(string message) : base(message) { }
        public DomainIsInInvalidStateException(string message, Exception inner) : base(message, inner) { }
    }
}
