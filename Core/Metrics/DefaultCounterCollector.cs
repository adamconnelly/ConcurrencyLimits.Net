namespace ConcurrencyLimits.Net.Core.Metrics
{
    /// <summary>
    /// A default implementation of a collector.
    /// </summary>
    public class DefaultCounterCollector : ICollector<ICounter>
    {
        /// <summary>
        /// Creates a new counter with the specified label values.
        /// </summary>
        /// <param name="labelValues">The label values.</param>
        /// <returns>The counter.</returns>
        public ICounter WithLabels(string[] labelValues)
        {
            return new DefaultCounter();
        }
    }
}