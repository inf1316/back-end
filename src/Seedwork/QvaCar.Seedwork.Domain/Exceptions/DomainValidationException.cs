using System;
using System.Collections.Generic;

namespace QvaCar.Seedwork.Domain
{
    public class DomainValidationException : DomainException
    {
        public IDictionary<string, string[]> Errors { get; }
        public DomainValidationException(): base("One or more validation failures have occurred.") 
        {
            Errors = new Dictionary<string, string[]>();
        }
        public DomainValidationException(string propertyName, string error) : base("One or more validation failures have occurred.") 
        {
            Errors = new Dictionary<string, string[]>() { [propertyName] = new[] { error } };
        }
        public DomainValidationException(string propertyName, string error, string message, Exception inner) : base(message, inner) {
            Errors = new Dictionary<string, string[]>() { [propertyName] = new[] { error } };
        }
    }
}
