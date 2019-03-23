namespace ConcurrencyLimits.Net.Core.Limits
{
    using System;
    using System.Threading;

    /// <summary>
    /// An implementation of a fixed limit (i.e. a hard limit that doesn't change over time).
    /// </summary>
    public class FixedLimit : ILimit
    {
        private readonly int limit;
        private int inFlight;

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedLimit" /> class.
        /// </summary>
        /// <param name="limit">The limit.</param>
        public FixedLimit(int limit)
        {
            this.limit = limit;
        }

        /// <summary>
        /// Gets the limit.
        /// </summary>
        public int Limit
        {
            get { return this.limit; }
        }

        /// <summary>
        /// Notifies the limit of the intention to start a new operation.
        /// </summary>
        /// <returns>
        /// Details about the operation, including whether it can proceed or not based on the limit.
        /// </returns>
        public OperationInfo NotifyStart()
        {
            var current = Interlocked.Increment(ref this.inFlight);

            return new OperationInfo(DateTime.Now, current <= this.limit);
        }

        /// <summary>
        /// Notifies the limit that a particular operation has completed.
        /// </summary>
        /// <param name="info">The information about the operation.</param>
        public void NotifyEnd(OperationInfo info)
        {
            Interlocked.Decrement(ref this.inFlight);
        }
    }
}