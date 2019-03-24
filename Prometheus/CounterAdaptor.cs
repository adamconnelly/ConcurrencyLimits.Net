namespace ConcurrencyLimits.Net.Prometheus
{
    using ConcurrencyLimits.Net.Core;

    /// <summary>
    /// Wraps a prometheus counter.
    /// </summary>
    public class CounterAdaptor : ICounter
    {
        private readonly global::Prometheus.ICounter counter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterAdaptor" /> class.
        /// </summary>
        /// <param name="counter">The counter to adapt.</param>
        public CounterAdaptor(global::Prometheus.ICounter counter)
        {
            this.counter = counter;
        }

        /// <summary>
        /// Increments the counter.
        /// </summary>
        public void Increment()
        {
            this.counter.Inc();
        }
    }
}