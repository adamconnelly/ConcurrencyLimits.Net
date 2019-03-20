namespace ConcurrencyLimits.Net.Core.Limits
{
    using System;
    using System.Threading;

    public class AlternativeFixedLimit : IAlternativeLimit
    {
        private readonly int limit;
        private int inFlight;

        public AlternativeFixedLimit(int limit)
        {
            this.limit = limit;
        }

        public OperationInfo NotifyStart()
        {
            var current = Interlocked.Increment(ref this.inFlight);

            return new OperationInfo(DateTime.Now, current <= this.limit);
        }

        // TODO: Maybe remove NotifyEnd and switch to Disposed pattern.
        public void NotifyEnd(OperationInfo info)
        {
            Interlocked.Decrement(ref this.inFlight);
        }
    }
}