using System;

namespace QvaCar.Infraestructure.Data.Elastic.Exceptions
{
    public class ExtensionFailException : Exception
    {
        public ExtensionFailException() { }
        public ExtensionFailException(string message) : base(message) { }
    }
}
