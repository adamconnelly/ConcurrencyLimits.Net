namespace ConcurrencyLimits.Net.Core
{
    /// <summary>
    /// Used to record a value that varies.
    /// </summary>
    public interface IGauge
    {
        /// <summary>
        /// Sets the current value of the gauge.
        /// </summary>
        /// <param name="value">The current value.</param>
        void Set(double value);
    }
}