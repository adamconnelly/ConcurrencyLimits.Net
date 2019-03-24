namespace ConcurrencyLimits.Net.Core
{
    /// <summary>
    /// Provides a way of recording metrics.
    /// </summary>
    public interface IMetricsRegistry
    {
        /// <summary>
        /// Creates a new counter.
        /// </summary>
        /// <param name="name">The name of the counter.</param>
        /// <param name="description">A description of the counter.</param>
        /// <param name="labels">The names of the labels to record.</param>
        /// <returns>The collector that can be used to record metric values.</returns>
        ICollector<ICounter> CreateCounter(
            string name, string description, string[] labels);

        /// <summary>
        /// Creates a new gauge.
        /// </summary>
        /// <param name="name">The name of the gauge.</param>
        /// <param name="description">A description of the gauge.</param>
        /// <param name="labels">The names of the labels to record.</param>
        /// <returns>The collector that can be used to record metric values.</returns>
        ICollector<IGauge> CreateGauge(string name, string description, string[] labels);
    }
}