namespace ConcurrencyLimits.Net.AspNetCore.Configuration
{
    using ConcurrencyLimits.Net.Core;
    using ConcurrencyLimits.Net.Core.Limits;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Used to configure limits.
    /// </summary>
    public class LimitConfigurer : ILimitConfigurer
    {
        private readonly IServiceCollection serviceCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitConfigurer" /> class.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        public LimitConfigurer(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
        }

        /// <inheritdoc/>
        public void WithFixedLimit(int limit)
        {
            this.serviceCollection.AddSingleton<ILimit>(p => new FixedLimit(limit));
        }
    }
}