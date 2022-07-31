using QvaCar.Application.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace QvaCar.Application
{
    public class Validate
    {
        public static void IsNotNull([NotNull] object? obj, string propertyName, string description) 
        {
            if (obj is null)
                throw new ValidationException(propertyName, description);
        }
    }
}
