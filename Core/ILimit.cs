namespace ConcurrencyLimits.Net.Core
{
    /// <summary>
    /// Represents the implementation of a limiting algorithm.
    /// </summary>
    public interface ILimit
    {
        /// <summary>
        /// Notifies the limit that an operation is starting.
        /// </summary>
        /// <returns>
        /// Information about the operation, along with whether or not it can proceed.
        /// </returns>
        /// <remarks>
        /// You *must* always call <see ref="NotifyEnd" /> once the operation has completed,
        /// otherwise the limit can get out of sync causing operations to be incorrectly limited.
        /// </remarks>
        OperationInfo NotifyStart();

        /// <summary>
        /// Notifies the limit that an operation has finished.
        /// </summary>
        /// <param name="info">
        /// The information about the operation being performed.
        /// </param>
        void NotifyEnd(OperationInfo info);
    }
}
