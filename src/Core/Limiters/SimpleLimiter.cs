using System;
using System.Threading;
using ConcurrencyLimits.Net.Core.Limits;

namespace ConcurrencyLimits.Net.Core.Limiters
{
    public class SimpleLimiter : ILimiter
    {
        private int inFlight;
        private ILimit limit;

        public SimpleLimiter(ILimit limit)
        {
            this.limit = limit;
        }

        public int InFlight { get { return this.inFlight; } }

        public string StateDescription => $"InFlight: {InFlight}; Limit: {this.limit.Limit}";

        /// <inheritdoc />
        public (bool canProceed, IListener listener) Acquire()
        {
            var newInflight = Interlocked.Increment(ref this.inFlight);
            bool canProceed = newInflight <= this.limit.Limit;
            
            // TODO: Rethink how all this hangs together, and maybe return no-op listener if can't proceed
            if (!canProceed) {
                this.Release();
            }

            return (canProceed, new Listener(this));
        }

        public void Release()
        {
            Interlocked.Decrement(ref this.inFlight);
        }

        public override string ToString()
        {
            return StateDescription;
        }
    }
}