namespace ConcurrencyLimits.Net.Core.Metrics
{
    /// <summary>
    /// A default implementation of a collector for gauges.
    /// </summary>
    public class DefaultGaugeCollector : ICollector<IGauge>
    {
        /// <summary>
        /// Creates a new gauge for the specified label values.
        /// </summary>
        /// <param name="labelValues">The label values.</param>
        /// <returns>The new gauge.</returns>
        public IGauge WithLabels(string[] labelValues)
        {
            return new DefaultGauge();
        }
    }
}