namespace ConcurrencyLimits.Net.Tests.Core.Limiters
{
    using System;
    using System.Threading.Tasks;
    using ConcurrencyLimits.Net.Core;
    using ConcurrencyLimits.Net.Core.Limiters;
    using Moq;
    using Xunit;

    public class SimpleLimiterTests
    {
        private readonly Mock<ILimit> limit;
        private readonly Mock<IMetricsRegistry> metricsRegistry;
        private readonly SimpleLimiter limiter;

        public SimpleLimiterTests()
        {
            this.limit = new Mock<ILimit>();
            this.metricsRegistry = new Mock<IMetricsRegistry> { DefaultValue = DefaultValue.Mock };
            this.limiter = new SimpleLimiter(this.limit.Object, this.metricsRegistry.Object);
        }

        [Fact]
        public async Task TryProcess_ExecutesOperation()
        {
            // Arrange
            this.AllowExecution();

            var hasProcessed = false;

            // Act
            await this.TryProcess(() => hasProcessed = true);

            // Assert
            Assert.True(hasProcessed);
        }

        [Fact]
        public async Task TryProcess_DoesNotProcessOperationIfLimitBreached()
        {
            // Arrange
            this.DenyExecution();

            var hasProcessed = false;

            // Act
            await this.TryProcess(() => hasProcessed = true);

            // Assert
            Assert.False(hasProcessed);
        }

        [Fact]
        public async Task TryProcess_CallsNotifyEnd()
        {
            // Arrange
            var info = this.AllowExecution();

            // Act
            await this.TryProcess(() => { });

            // Assert
            this.limit.Verify(l => l.NotifyEnd(info));
        }

        [Fact]
        public async Task TryProcess_CallsNotifyEndWhenLimitIsBreached()
        {
            // Arrange
            var info = this.DenyExecution();

            // Act
            await this.TryProcess(() => { });

            // Assert
            this.limit.Verify(l => l.NotifyEnd(info));
        }

        [Fact]
        public async Task TryProcess_CallsNotifyEndWhenOperationThrowsException()
        {
            // Arrange
            var info = this.AllowExecution();

            // Act
            try
            {
                await this.TryProcess(() => throw new Exception("Operation failed!"));
            }
            catch (Exception)
            {
                // Ignore test exception
            }

            // Assert
            this.limit.Verify(l => l.NotifyEnd(info));
        }

        [Fact]
        public async Task TryProcess_IndicatesWhenOperationProcessed()
        {
            // Arrange
            this.AllowExecution();

            // Act
            var processed = await this.TryProcess(() => { });

            // Assert
            Assert.True(processed);
        }

        [Fact]
        public async Task TryProcess_IndicatesWhenOperationNotProcessed()
        {
            // Arrange
            this.DenyExecution();

            // Act
            var processed = await this.TryProcess(() => { });

            // Assert
            Assert.False(processed);
        }

        private Task<bool> TryProcess(Action operation)
        {
            return this.limiter.TryProcess(() =>
            {
                operation();
                return Task.FromResult(0);
            });
        }

        private OperationInfo AllowExecution()
        {
            var info = new OperationInfo(DateTime.Now, true);
            this.limit.Setup(l => l.NotifyStart()).Returns(info);

            return info;
        }

        private OperationInfo DenyExecution()
        {
            var info = new OperationInfo(DateTime.Now, false);
            this.limit.Setup(l => l.NotifyStart()).Returns(info);

            return info;
        }
    }
}