namespace ConcurrencyLimits.Net.Tests.AspNetCore.Configuration
{
    using ConcurrencyLimits.Net.AspNetCore.Configuration;
    using ConcurrencyLimits.Net.Core;
    using ConcurrencyLimits.Net.Core.Limits;
    using ConcurrencyLimits.Net.Core.Metrics;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class LimitConfigurerTests
    {
        [Fact]
        public void WithFixedLimit_ConfiguresAFixedLimit()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IMetricsRegistry>(p => new DefaultMetricsRegistry());
            var configurer = new LimitConfigurer(serviceCollection);

            // Act
            configurer.WithFixedLimit(10);

            // Assert
            var provider = serviceCollection.BuildServiceProvider();

            var limit = Assert.IsType<FixedLimit>(provider.GetService<ILimit>());
            Assert.Equal(10, limit.Limit);
        }
    }
}