using System;

namespace QvaCar.Seedwork.Domain
{
    public class DomainInvalidOperationException : DomainException
    {
        public DomainInvalidOperationException() { }
        public DomainInvalidOperationException(string message) : base(message) { }
        public DomainInvalidOperationException(string message, Exception inner) : base(message, inner) { }       
    }
}
