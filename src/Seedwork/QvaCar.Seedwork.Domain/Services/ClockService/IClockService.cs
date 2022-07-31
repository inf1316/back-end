using System;

namespace QvaCar.Seedwork.Domain.Services
{
    public interface IClockService
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
