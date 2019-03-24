namespace ConcurrencyLimits.Net.Tests.Core.Limiters
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using ConcurrencyLimits.Net.Core;
    using ConcurrencyLimits.Net.Core.Limiters;
    using ConcurrencyLimits.Net.Core.Limits;
    using Moq;
    using Xunit;

    public class SimpleLimiterMetricTests
    {
        [Fact]
        public void Constructor_CreatesTheExecutedMetric()
        {
            // Arrange
            var metricsRegistry = new Mock<IMetricsRegistry> { DefaultValue = DefaultValue.Mock };
            var limit = new FixedLimit(5, metricsRegistry.Object);

            // Act
            var limiter = new SimpleLimiter(limit, metricsRegistry.Object);

            // Assert
            metricsRegistry.Verify(r => r.CreateCounter(
                SimpleLimiter.Metrics.Executed.Name,
                SimpleLimiter.Metrics.Executed.Description,
                new[] { SimpleLimiter.Metrics.Labels.LimiterTypeName }));
        }

        [Fact]
        public async Task TryProcess_IncrementsExecutedMetric()
        {
            // Arrange
            var metricsRegistry = new Mock<IMetricsRegistry> { DefaultValue = DefaultValue.Mock };
            var limit = new FixedLimit(5, metricsRegistry.Object);

            var operationsExecuted = new Mock<ICounter>();
            metricsRegistry.Setup(r => r.CreateCounter(
                    SimpleLimiter.Metrics.Executed.Name,
                    It.IsAny<string>(),
                    It.IsAny<string[]>())
                .WithLabels(It.IsAny<string[]>())).Returns(operationsExecuted.Object);

            var limiter = new SimpleLimiter(limit, metricsRegistry.Object);

            // Act
            await limiter.TryProcess(() => Task.FromResult(0));
            await limiter.TryProcess(() => Task.FromResult(0));
            await limiter.TryProcess(() => Task.FromResult(0));
            await limiter.TryProcess(() => Task.FromResult(0));

            // Assert
            operationsExecuted.Verify(c => c.Increment(), Times.Exactly(4));
        }

        [Fact]
        public void Constructor_CreatesTheLimitedMetric()
        {
            // Arrange
            var metricsRegistry = new Mock<IMetricsRegistry> { DefaultValue = DefaultValue.Mock };
            var limit = new FixedLimit(5, metricsRegistry.Object);

            // Act
            var limiter = new SimpleLimiter(limit, metricsRegistry.Object);

            // Assert
            metricsRegistry.Verify(r => r.CreateCounter(
                SimpleLimiter.Metrics.Limited.Name,
                SimpleLimiter.Metrics.Limited.Description,
                new[] { SimpleLimiter.Metrics.Labels.LimiterTypeName }));
        }

        [Fact]
        public void TryProcess_IncrementsLimitedMetric()
        {
            // Arrange
            var metricsRegistry = new Mock<IMetricsRegistry> { DefaultValue = DefaultValue.Mock };
            var limit = new FixedLimit(2, metricsRegistry.Object);

            var operationsLimited = new Mock<ICounter>();
            metricsRegistry.Setup(r => r.CreateCounter(
                    SimpleLimiter.Metrics.Limited.Name,
                    It.IsAny<string>(),
                    It.IsAny<string[]>())
                .WithLabels(It.IsAny<string[]>())).Returns(operationsLimited.Object);

            var limiter = new SimpleLimiter(limit, metricsRegistry.Object);

            var requestBlocker = new ManualResetEvent(false);
            var taskExecutedWaitHandles = new List<WaitHandle>
            {
                this.ProcessBlockingTask(limiter, requestBlocker),
                this.ProcessBlockingTask(limiter, requestBlocker)
            };

            WaitHandle.WaitAll(taskExecutedWaitHandles.ToArray());

            // Act
            // Temporarily ignore the warning about not awaiting Tasks since we actually
            // want the tasks to block so that we can build up the current operation count
            #pragma warning disable CS4014

            // Limit reached - the following requests will be limited
            limiter.TryProcess(() => Task.FromResult(0));
            limiter.TryProcess(() => Task.FromResult(0));
            #pragma warning restore CS4014

            // Assert
            operationsLimited.Verify(c => c.Increment(), Times.Exactly(2));

            requestBlocker.Set();
        }

        /// <summary>
        /// Calls <see cref="SimpleLimiter.TryProcess(System.Func{Task})" /> with an
        /// operation that blocks until <paramref ref="blockUntil" /> is signalled.
        /// </summary>
        /// <param name="limiter">The limiter.</param>
        /// <param name="blockUntil">
        /// A <see cref="WaitHandle" /> handle that signals when the operation can complete.
        /// </param>
        /// <returns>
        /// A <see cref="WaitHandle" /> that can be waited on to ensure that the
        /// operation has begun executing before continuing.
        /// </returns>
        private WaitHandle ProcessBlockingTask(SimpleLimiter limiter, WaitHandle blockUntil)
        {
            var executedHandle = new ManualResetEvent(false);
            Task.Run(() => limiter.TryProcess(() =>
            {
                executedHandle.Set();
                blockUntil.WaitOne();

                return Task.FromResult(0);
            }));

            return executedHandle;
        }
    }
}