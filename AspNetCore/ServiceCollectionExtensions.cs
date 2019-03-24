namespace ConcurrencyLimits.Net.AspNetCore
{
    using ConcurrencyLimits.Net.AspNetCore.Configuration;
    using ConcurrencyLimits.Net.Core;
    using ConcurrencyLimits.Net.Core.Metrics;
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
            services.AddSingleton<IMetricsRegistry>(p => new DefaultMetricsRegistry());

            return new LimiterConfigurer(services);
        }
    }
}