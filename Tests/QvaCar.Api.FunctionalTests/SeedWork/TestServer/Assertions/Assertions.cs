using System;
using System.Threading.Tasks;

namespace QvaCar.Api.FunctionalTests.SeedWork
{
    public static class Assertions
    {
        public static async Task AssertAsyncOperation(int repeatTimes, int delayBetweenAssertionInMiliseconds, Func<Task> assertion)
        {
            Exception? lastException = null;
            for (int i = 1; i <= repeatTimes; i++)
            {
                try
                {
                    await assertion();
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    if (i != repeatTimes)
                        await Task.Delay(delayBetweenAssertionInMiliseconds);
                }
            }
            throw lastException ?? new ArgumentException("Must be greather than 0", nameof(repeatTimes));
        }

        public static async Task AssertAsyncOperation(int repeatTimes, int delayBetweenAssertionInMiliseconds, Action assertion)
        {
            await AssertAsyncOperation(repeatTimes, delayBetweenAssertionInMiliseconds, () =>
            {
                assertion();
                return Task.CompletedTask;
            });
        }
    }
}
