namespace ConcurrencyLimits.Net.Tests.AspNetCore.Configuration
{
    using ConcurrencyLimits.Net.AspNetCore;
    using ConcurrencyLimits.Net.AspNetCore.Configuration;
    using ConcurrencyLimits.Net.Core;
    using ConcurrencyLimits.Net.Core.Limiters;
    using ConcurrencyLimits.Net.Core.Limits;
    using ConcurrencyLimits.Net.Core.Metrics;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Xunit;

    public class LimiterConfigurerTests
    {
        private readonly IServiceCollection serviceCollection = new ServiceCollection();
        private readonly IMetricsRegistry metricsRegistry = new DefaultMetricsRegistry();
        private readonly LimiterConfigurer configurer;

        public LimiterConfigurerTests()
        {
            this.serviceCollection.AddSingleton<IMetricsRegistry>(p => this.metricsRegistry);
            this.configurer = new LimiterConfigurer(this.serviceCollection);
        }

        [Fact]
        public void UseSimpleLimiter_ConfiguresSimpleLimiter()
        {
            // Arrange
            // Add a limit to allow the limiter to be resolved
            this.serviceCollection.AddSingleton(
                typeof(ILimit),
                p => new FixedLimit(10, this.metricsRegistry));

            // Act
            this.configurer.UseSimpleLimiter();

            // Assert
            var provider = this.serviceCollection.BuildServiceProvider();
            var limiter = provider.GetService<ILimiter>();
            Assert.IsType<SimpleLimiter>(limiter);
        }

        [Fact]
        public void UseSimpleLimiter_ReturnsLimitConfigurer()
        {
            // Act
            var limitConfigurer = this.configurer.UseSimpleLimiter();

            // Assert
            Assert.IsAssignableFrom<ILimitConfigurer>(limitConfigurer);
        }
    }
}