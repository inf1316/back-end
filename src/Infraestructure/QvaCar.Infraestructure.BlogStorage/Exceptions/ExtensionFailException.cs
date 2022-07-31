using System;

namespace QvaCar.Infraestructure.BlogStorage.Exceptions
{
    public class ExtensionFailException : Exception
    {
        public ExtensionFailException() { }
        public ExtensionFailException(string message) : base(message) { }
    }
}
