﻿using System;

namespace QvaCar.Application.Common.Secutity
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class AuthorizeAttribute : Attribute
    {
        public AuthorizeAttribute() { }

        public string? Roles { get; set; }

        public string? Policy { get; set; }
    }
}
