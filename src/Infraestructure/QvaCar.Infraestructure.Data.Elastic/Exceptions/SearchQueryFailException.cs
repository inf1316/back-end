using System;

namespace QvaCar.Infraestructure.Data.Elastic.Exceptions
{
    public class SearchQueryFailException : Exception
    {
        public SearchQueryFailException() { }
        public SearchQueryFailException(string message) : base(message) { }
        public SearchQueryFailException(string message, Exception inner) : base(message, inner) { }
        protected SearchQueryFailException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
