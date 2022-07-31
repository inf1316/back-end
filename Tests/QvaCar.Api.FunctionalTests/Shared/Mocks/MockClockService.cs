using QvaCar.Seedwork.Domain.Services;
using System;

namespace QvaCar.Api.FunctionalTests.SeedWork
{
    internal class MockClockService : IClockService
    {
        private static MockClockService? _service;
        private static DateTime _now = DateTime.MinValue;
        private static DateTime _utcNow = DateTime.MinValue;

        private MockClockService() { }
        public static MockClockService Service
        {
            get
            {
                _service ??= new MockClockService();
                return _service;
            }
        }
        public static void AssumeNowAs(DateTime nowDateTime) => _now = nowDateTime;
        public static void AssumeUtcNowAs(DateTime nowUtcDateTime) => _utcNow = nowUtcDateTime;
        public static void ResetService()
        {
            _now = DateTime.MinValue;
            _utcNow = DateTime.MinValue;
        }

        public DateTime Now => _now;

        public DateTime UtcNow => _utcNow;
    }
}
