using System;

namespace QvaCar.Seedwork.Domain
{
    public static class WorkaroundExtensions
    {
        public static T CloneBecauseEfCoreRestriction<T>(this T source) where T : Enumeration
        {
            return (T)(Activator.CreateInstance(typeof(T), new object[] { source.Id, source.Name }) ?? throw new InvalidCastException("Cannot Construct object"));
        }
    }
}