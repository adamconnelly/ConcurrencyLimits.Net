namespace ConcurrencyLimits.Net.AspNetCore.Configuration
{
    using ConcurrencyLimits.Net.Core;
    using ConcurrencyLimits.Net.Core.Limits;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class LimitConfigurerTests
    {
        [Fact]
        public void WithFixedLimit_ConfiguresAFixedLimit()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
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