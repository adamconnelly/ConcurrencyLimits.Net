namespace ConcurrencyLimits.Net.Tests.Core.Limiters
{
    using System;
    using System.Threading.Tasks;
    using ConcurrencyLimits.Net.Core;
    using ConcurrencyLimits.Net.Core.Limiters;
    using Moq;
    using Xunit;

    public class LimiterTests
    {
        private readonly Mock<ILimit> limit;
        private readonly SimpleLimiter limiter;

        public LimiterTests()
        {
            this.limit = new Mock<ILimit>();
            this.limiter = new SimpleLimiter(this.limit.Object);
        }

        [Fact]
        public async Task TryProcess_ExecutesOperation()
        {
            // Arrange
            this.limit.Setup(l => l.NotifyStart()).Returns(new OperationInfo(DateTime.Now, true));

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
            this.limit.Setup(l => l.NotifyStart()).Returns(new OperationInfo(DateTime.Now, false));

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
            var info = new OperationInfo(DateTime.Now, true);
            this.limit.Setup(l => l.NotifyStart()).Returns(info);

            // Act
            await this.TryProcess(() => { });

            // Assert
            this.limit.Verify(l => l.NotifyEnd(info));
        }

        [Fact]
        public async Task TryProcess_CallsNotifyEndWhenLimitIsBreached()
        {
            // Arrange
            var info = new OperationInfo(DateTime.Now, false);
            this.limit.Setup(l => l.NotifyStart()).Returns(info);

            // Act
            await this.TryProcess(() => { });

            // Assert
            this.limit.Verify(l => l.NotifyEnd(info));
        }

        [Fact]
        public async Task TryProcess_CallsNotifyEndWhenOperationThrowsException()
        {
            // Arrange
            var info = new OperationInfo(DateTime.Now, true);
            this.limit.Setup(l => l.NotifyStart()).Returns(info);

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
            this.limit.Setup(l => l.NotifyStart()).Returns(new OperationInfo(DateTime.Now, true));

            // Act
            var processed = await this.TryProcess(() => { });

            // Assert
            Assert.True(processed);
        }

        [Fact]
        public async Task TryProcess_IndicatesWhenOperationNotProcessed()
        {
            // Arrange
            this.limit.Setup(l => l.NotifyStart()).Returns(new OperationInfo(DateTime.Now, false));

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
    }
}