namespace ConcurrencyLimits.Net.Core
{
    /// <summary>
    /// Used as a way of creating a more specific metric with label values specified.
    /// </summary>
    /// <typeparam name="T">The type of metric.</typeparam>
    public interface ICollector<T>
    {
        /// <summary>
        /// Sets the label values for the metric and provides the metric instance.
        /// </summary>
        /// <param name="labelValues">The label values.</param>
        /// <returns>The metric instance.</returns>
        T WithLabels(string[] labelValues);
    }
}