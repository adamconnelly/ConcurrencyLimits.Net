namespace ConcurrencyLimits.Net.Core
{
    /// <summary>
    /// Represents an algorithm for limiting concurrency.
    /// </summary>
    public interface ILimit
    {
        /// <summary>
        /// Gets the current limit.
        /// </summary>
        int Limit { get; }
    }
}