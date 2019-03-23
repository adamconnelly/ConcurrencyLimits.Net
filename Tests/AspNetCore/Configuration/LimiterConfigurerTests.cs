namespace ConcurrencyLimits.Net.Tests.AspNetCore.Configuration
{
    using ConcurrencyLimits.Net.AspNetCore;
    using ConcurrencyLimits.Net.AspNetCore.Configuration;
    using ConcurrencyLimits.Net.Core;
    using ConcurrencyLimits.Net.Core.Limiters;
    using ConcurrencyLimits.Net.Core.Limits;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Xunit;

    public class LimiterConfigurerTests
    {
        [Fact]
        public void UseSimpleLimiter_ConfiguresSimpleLimiter()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var configurer = new LimiterConfigurer(serviceCollection);

            // Add a limit to allow the limiter to be resolved
            serviceCollection.AddSingleton(typeof(ILimit), p => new FixedLimit(10));

            // Act
            configurer.UseSimpleLimiter();

            // Assert
            var provider = serviceCollection.BuildServiceProvider();
            var limiter = provider.GetService<ILimiter>();
            Assert.IsType<SimpleLimiter>(limiter);
        }

        [Fact]
        public void UseSimpleLimiter_ReturnsLimitConfigurer()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var configurer = new LimiterConfigurer(serviceCollection);

            // Act
            var limitConfigurer = configurer.UseSimpleLimiter();

            // Assert
            Assert.IsAssignableFrom<ILimitConfigurer>(limitConfigurer);
        }
    }
}