namespace ConcurrencyLimits.Net.Core.Limiters
{
    using System;
    using System.Threading.Tasks;

    public class SimpleLimiter : ILimiter
    {
        private readonly ILimit limit;

        public SimpleLimiter(ILimit limit)
        {
            this.limit = limit;
        }

        public async Task<bool> TryProcess(Func<Task> operation)
        {
            var operationInfo = this.limit.NotifyStart();

            try
            {
                if (operationInfo.CanProcess)
                {
                    await operation();
                }

                return operationInfo.CanProcess;
            }
            finally
            {
                this.limit.NotifyEnd(operationInfo);
            }
        }
    }
}