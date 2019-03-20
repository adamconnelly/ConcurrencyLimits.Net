namespace ConcurrencyLimits.Net.Core.Limiters
{
    using System;
    using System.Threading.Tasks;

    public class AlternativeLimiter : IAlternativeLimiter
    {
        private readonly IAlternativeLimit limit;

        public AlternativeLimiter(IAlternativeLimit limit)
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