using System;

namespace QvaCar.Seedwork.Domain.Services
{
    public class ClockService : IClockService
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
