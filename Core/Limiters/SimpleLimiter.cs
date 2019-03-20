namespace ConcurrencyLimits.Net.Core.Limiters
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// A limiter that attempts to process an operation based on the limit being used.
    /// </summary>
    public class SimpleLimiter : ILimiter
    {
        private readonly ILimit limit;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleLimiter"/> class.
        /// </summary>
        /// <param name="limit">The limit to use.</param>
        public SimpleLimiter(ILimit limit)
        {
            this.limit = limit;
        }

        /// <inheritdoc/>
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