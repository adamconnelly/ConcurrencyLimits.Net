namespace ConcurrencyLimits.Net.Core.Metrics
{
    /// <summary>
    /// A default implementation of a metrics registry.
    /// </summary>
    public class DefaultMetricsRegistry : IMetricsRegistry
    {
        /// <summary>
        /// Creates a new counter.
        /// </summary>
        /// <param name="name">The name of the counter.</param>
        /// <param name="description">A description of the counter.</param>
        /// <param name="labels">The label names.</param>
        /// <returns>A collector object that can be used to record metrics.</returns>
        public ICollector<ICounter> CreateCounter(string name, string description, string[] labels)
        {
            return new DefaultCounterCollector();
        }

        /// <summary>
        /// Creates a new gauge.
        /// </summary>
        /// <param name="name">The name of the gauge.</param>
        /// <param name="description">A description of the gauge.</param>
        /// <param name="labels">The label names.</param>
        /// <returns>A collector object that can be used to record metrics.</returns>
        public ICollector<IGauge> CreateGauge(string name, string description, string[] labels)
        {
            return new DefaultGaugeCollector();
        }
    }
}