namespace ConcurrencyLimits.Net.Tests.Core.Limits
{
    using ConcurrencyLimits.Net.Core;
    using ConcurrencyLimits.Net.Core.Limits;
    using Moq;
    using Xunit;

    public class FixedLimitTests
    {
        [Fact]
        public void NotifyStart_IndicatesOperationCanProceed()
        {
            // Arrange
            var limit = GetLimit(5);

            // Act
            var info = limit.NotifyStart();

            // Assert
            Assert.True(info.CanProcess);
        }

        [Fact]
        public void NotifyStart_IndicatesOperationCannotProceedWhenLimitBreached()
        {
            // Arrange
            var limit = GetLimit(3);
            limit.NotifyStart();
            limit.NotifyStart();
            limit.NotifyStart();

            // Act
            var info = limit.NotifyStart();

            // Assert
            Assert.False(info.CanProcess);
        }

        [Fact]
        public void NotifyStart_IndicatesOperationCanProceedWhenLimitReached()
        {
            // In this test we should be allowed to proceed because there is a limit
            // of 3, and we notify the start of 3 operations.

            // Arrange
            var limit = GetLimit(3);
            limit.NotifyStart();
            limit.NotifyStart();

            // Act
            var info = limit.NotifyStart();

            // Assert
            Assert.True(info.CanProcess);
        }

        [Fact]
        public void NotifyEnd_AllowsAnotherRequestToBeProcessed()
        {
            // Arrange
            var limit = GetLimit(2);
            limit.NotifyStart();
            var info = limit.NotifyStart();

            // Act
            limit.NotifyEnd(info);
            info = limit.NotifyStart();

            // Assert
            Assert.True(info.CanProcess);
        }

        private static FixedLimit GetLimit(int limit)
        {
            var registry = new Mock<IMetricsRegistry> { DefaultValue = DefaultValue.Mock };
            return new FixedLimit(limit, registry.Object);
        }
    }
}