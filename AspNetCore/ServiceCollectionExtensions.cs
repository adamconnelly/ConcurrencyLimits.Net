namespace ConcurrencyLimits.Net.AspNetCore
{
    using ConcurrencyLimits.Net.AspNetCore.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extension methods for <see cref="IServiceCollection" />
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Begins configuring the services used by the concurrency limiting middleware.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The limiter configurer.</returns>
        public static ILimiterConfigurer AddConcurrencyLimits(this IServiceCollection services)
        {
            return new LimiterConfigurer(services);
        }
    }
}