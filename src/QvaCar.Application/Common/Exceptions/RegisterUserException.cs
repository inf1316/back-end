using System;
using System.Collections.Generic;
using System.Linq;

namespace QvaCar.Application.Exceptions
{
    public class RegisterUserException : Exception
    {
        public List<string> Errors { get; }
        public RegisterUserException()
            : base("Fail to create user.")
        {
            Errors = new List<string>();
        }

        public RegisterUserException(IEnumerable<string> failures)
            : this()
        {
            Errors = failures.ToList();
        }
    }
}