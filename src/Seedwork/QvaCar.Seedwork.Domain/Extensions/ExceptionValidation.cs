using System;
using System.Diagnostics.CodeAnalysis;

namespace QvaCar.Seedwork.Domain
{

    public static class Check
    {
        public static void IsNotNull<TException>([NotNull] object? obj) where TException : Exception
        {
            if (obj is null)
            {
                var ex = Activator.CreateInstance(typeof(TException), obj) as Exception;
                throw ex ?? new NullReferenceException($"Object should not be null");
            }
                
        }

        public static void IsNotNull<TException>([NotNull] object? obj, string message) where TException : Exception
        {
            if (obj is null)
            {
                var ex = Activator.CreateInstance(typeof(TException), message) as Exception;
                throw ex ?? new NullReferenceException(message);
            }
        }

        public static void That<TException>(bool condition, string message = "") where TException : Exception
        {
            if (!condition)
            {
                var ex = Activator.CreateInstance(typeof(TException), message) as Exception;
                throw ex ?? new NullReferenceException(message);
            }
        }

        public static void That<TException>(bool condition, Func<TException> configureException) where TException : Exception
        {
            if (!condition)
                throw configureException();
        }       
    }
}