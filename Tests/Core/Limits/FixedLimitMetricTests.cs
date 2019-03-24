namespace ConcurrencyLimits.Net.Tests.Core
{
    using ConcurrencyLimits.Net.Core;
    using ConcurrencyLimits.Net.Core.Limits;
    using Moq;
    using Xunit;

    public class FixedLimitMetricTests
    {
        [Fact]
        public void Constructor_CreatesGaugeWithDetails()
        {
            // Arrange
            var registry = new Mock<IMetricsRegistry> { DefaultValue = DefaultValue.Mock };

            // Act
            var limit = new FixedLimit(5, registry.Object);

            // Assert
            registry.Verify(r => r.CreateGauge(
                FixedLimit.Metrics.MaxOperations.Name,
                FixedLimit.Metrics.MaxOperations.Description,
                new[] { FixedLimit.Metrics.Labels.LimitTypeName }));
        }

        [Fact]
        public void Constructor_SetsMetricValue()
        {
            // Arrange
            var registry = new Mock<IMetricsRegistry>();
            var collector = new Mock<ICollector<IGauge>> { DefaultValue = DefaultValue.Mock };
            registry.Setup(r => r.CreateGauge(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>())).Returns(collector.Object);

            // Act
            var limit = new FixedLimit(5, registry.Object);

            // Assert
            collector.Verify(c => c.WithLabels(new[] { FixedLimit.Metrics.Labels.LimitTypeValue }).Set(5));
        }

        [Fact]
        public void Constructor_CreatesTheCurrentOperationMetric()
        {
            // Arrange
            var registry = new Mock<IMetricsRegistry> { DefaultValue = DefaultValue.Mock };

            // Act
            var limit = new FixedLimit(5, registry.Object);

            // Assert
            registry.Verify(r => r.CreateGauge(
                FixedLimit.Metrics.CurrentOperations.Name,
                FixedLimit.Metrics.CurrentOperations.Description,
                new[] { FixedLimit.Metrics.Labels.LimitTypeName }).WithLabels(new[] { FixedLimit.Metrics.Labels.LimitTypeValue }));
        }

        [Fact]
        public void NotifyStart_IncrementsCurrentOperationMetric()
        {
            // Arrange
            var registry = new Mock<IMetricsRegistry> { DefaultValue = DefaultValue.Mock };

            var currentOperations = new Mock<IGauge>();
            registry.Setup(r => r.CreateGauge(
                        FixedLimit.Metrics.CurrentOperations.Name,
                        It.IsAny<string>(),
                        It.IsAny<string[]>())
                    .WithLabels(It.IsAny<string[]>()))
                .Returns(currentOperations.Object);

            var limit = new FixedLimit(5, registry.Object);

            // Act
            limit.NotifyStart();
            limit.NotifyStart();
            limit.NotifyStart();

            // Assert
            currentOperations.Verify(c => c.Set(3));
        }

        [Fact]
        public void NotifyEnd_SetsCurrentOperationMetric()
        {
            // Arrange
            var registry = new Mock<IMetricsRegistry> { DefaultValue = DefaultValue.Mock };

            var currentOperations = new Mock<IGauge>();
            registry.Setup(r => r.CreateGauge(
                        FixedLimit.Metrics.CurrentOperations.Name,
                        It.IsAny<string>(),
                        It.IsAny<string[]>())
                    .WithLabels(It.IsAny<string[]>()))
                .Returns(currentOperations.Object);

            var limit = new FixedLimit(5, registry.Object);

            // Act
            limit.NotifyStart();
            limit.NotifyStart();
            var operationInfo = limit.NotifyStart();
            limit.NotifyEnd(operationInfo);

            // Assert
            currentOperations.Verify(c => c.Set(2), Times.Exactly(2));
        }
    }
}