namespace ConcurrencyLimits.Net.Core
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Enforces a concurrency limit.
    /// </summary>
    public interface ILimiter
    {
        /// <summary>
        /// Attempts to execute the specified operation if the limit has not been reached.
        /// </summary>
        /// <param name="operation">
        /// The operation to execute.
        /// </param>
        /// <returns>
        /// True if the operation was executed, false otherwise.
        /// </returns>
        Task<bool> TryProcess(Func<Task> operation);
    }
}