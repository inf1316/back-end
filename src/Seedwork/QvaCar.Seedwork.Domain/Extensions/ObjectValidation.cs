namespace QvaCar.Seedwork.Domain
{
    public static class ObjectValidation
    {
        public static bool IsNotNull(this object? obj) => obj is not null;

        public static bool IsNull(this object? obj) => obj is null;
    }
}
