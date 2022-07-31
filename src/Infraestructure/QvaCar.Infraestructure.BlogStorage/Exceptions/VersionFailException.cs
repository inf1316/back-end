using System;

namespace QvaCar.Infraestructure.BlogStorage.Exceptions
{
    public class VersionFailException : Exception
    {
        public VersionFailException() { }
        public VersionFailException(string message) : base(message) { }
    }
}
