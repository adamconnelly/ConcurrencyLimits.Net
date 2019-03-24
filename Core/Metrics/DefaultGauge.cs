namespace ConcurrencyLimits.Net.Core.Metrics
{
    /// <summary>
    /// A default implementation of a gauge.
    /// </summary>
    public class DefaultGauge : IGauge
    {
        /// <summary>
        /// Sets the current value of the gauge.
        /// </summary>
        /// <param name="value">The value to update the gauge to.</param>
        public void Set(double value)
        {
        }
    }
}