namespace ConcurrencyLimits.Net.AspNetCore.Configuration
{
    using ConcurrencyLimits.Net.Core;
    using ConcurrencyLimits.Net.Core.Limiters;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Used to configure the limiter to use in the ASP.NET Core pipeline.
    /// </summary>
    public class LimiterConfigurer : ILimiterConfigurer
    {
        private readonly IServiceCollection serviceCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LimiterConfigurer" /> class.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        public LimiterConfigurer(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
        }

        /// <inheritdoc/>
        public ILimitConfigurer UseSimpleLimiter()
        {
            this.serviceCollection.AddSingleton(typeof(ILimiter), typeof(SimpleLimiter));
            return new LimitConfigurer(this.serviceCollection);
        }
    }
}