namespace ConcurrencyLimits.Net.AspNetCore
{
    using ConcurrencyLimits.Net.Core;
    using ConcurrencyLimits.Net.Core.Limiters;

    /// <summary>
    /// Used to configure the <see cref="ILimiter" /> to use.
    /// </summary>
    public interface ILimiterConfigurer
    {
        /// <summary>
        /// Configures the application to use the <see cref="SimpleLimiter" />.
        /// </summary>
        /// <returns>
        /// The <see cref="ILimitConfigurer" /> to allow the limit to be configured.
        /// </returns>
        ILimitConfigurer UseSimpleLimiter();
    }
}