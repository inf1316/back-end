﻿using System;

namespace QvaCar.Seedwork.Domain
{

    public class DomainException : Exception
    {
        public DomainException() { }
        public DomainException(string message) : base(message) { }
        public DomainException(string message, Exception inner) : base(message, inner) { }       
    }
}
