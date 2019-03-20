namespace ConcurrencyLimits.Net.Core
{
    using System;

    /// <summary>
    /// Provides information about an in-process operation.
    /// </summary>
    public class OperationInfo
    {
        public OperationInfo(DateTime startTime, bool canProcess)
        {
            this.StartTime = startTime;
            this.CanProcess = canProcess;
        }

        /// <summary>
        /// Gets the time that the operation started.
        /// </summary>
        public DateTime StartTime { get; }

        /// <summary>
        /// Gets a value indicating whether the operation can be processed.
        /// If false, it indicates that the limit has been breached.
        /// </summary>
        public bool CanProcess { get; }
    }
}