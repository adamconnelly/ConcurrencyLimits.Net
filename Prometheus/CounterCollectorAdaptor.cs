namespace ConcurrencyLimits.Net.Prometheus
{
    using ConcurrencyLimits.Net.Core;
    using Prometheus;

    /// <summary>
    /// Wraps the prometheus counter object used to specify labels.
    /// </summary>
    public class CounterCollectorAdaptor : ICollector<ICounter>
    {
        private readonly global::Prometheus.Counter counter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterCollectorAdaptor" /> class.
        /// </summary>
        /// <param name="counter">The counter to wrap.</param>
        public CounterCollectorAdaptor(global::Prometheus.Counter counter)
        {
            this.counter = counter;
        }

        /// <summary>
        /// Creates a new counter with the specified label values.
        /// </summary>
        /// <param name="labelValues">The label values.</param>
        /// <returns>The counter.</returns>
        public ICounter WithLabels(string[] labelValues)
        {
            return new CounterAdaptor(this.counter.WithLabels(labelValues));
        }
    }
}