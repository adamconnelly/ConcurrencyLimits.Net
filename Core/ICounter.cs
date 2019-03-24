namespace ConcurrencyLimits.Net.Core
{
    /// <summary>
    /// Used to record ever increasing counts.
    /// </summary>
    public interface ICounter
    {
        /// <summary>
        /// Increments the current value.
        /// </summary>
        void Increment();
    }
}