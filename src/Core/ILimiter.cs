namespace ConcurrencyLimits.Net.Core
{
    /// <summary>
    /// Enforces a concurrency limit.
    /// </summary>
    public interface ILimiter
    {
        /// <summary>
        /// Attempts to acquire the ability to process a request.
        /// </summary>
        /// <returns>
        /// canProceed - true when the request can be processed, false otherwise;
        /// listener - used to notify the limiter when processing is complete.
        /// </returns>
        (bool canProceed, IListener listener) Acquire();

        void Release();

        /// <summary>
        /// Gets a description of the current state of the limiter.
        /// </summary>
        string StateDescription { get; }
    }
}