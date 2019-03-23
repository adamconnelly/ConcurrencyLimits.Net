namespace ConcurrencyLimits.Net.AspNetCore
{
    using ConcurrencyLimits.Net.Core.Limits;

    /// <summary>
    /// Allows limits to be configured.
    /// </summary>
    public interface ILimitConfigurer
    {
        /// <summary>
        /// Configures the limiter to use a <see cref="FixedLimit" />
        /// </summary>
        /// <param name="limit">The request limit.</param>
        void WithFixedLimit(int limit);
    }
}