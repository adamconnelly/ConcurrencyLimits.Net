namespace ConcurrencyLimits.Net.Core.Limiters
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// A limiter that attempts to process an operation based on the limit being used.
    /// </summary>
    public class SimpleLimiter : ILimiter
    {
        private readonly ILimit limit;
        private readonly ICounter operationsExecuted;
        private readonly ICounter operationsLimited;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleLimiter"/> class.
        /// </summary>
        /// <param name="limit">The limit to use.</param>
        /// <param name="metricsRegistry">The metrics registry.</param>
        public SimpleLimiter(ILimit limit, IMetricsRegistry metricsRegistry)
        {
            this.limit = limit;

            this.operationsExecuted = metricsRegistry.CreateCounter(
                    Metrics.Executed.Name,
                    Metrics.Executed.Description,
                    new[] { Metrics.Labels.LimiterTypeName })
                .WithLabels(new[] { Metrics.Labels.LimiterTypeDescription });
            this.operationsLimited = metricsRegistry.CreateCounter(
                    Metrics.Limited.Name,
                    Metrics.Limited.Description,
                    new[] { Metrics.Labels.LimiterTypeName })
                .WithLabels(new[] { Metrics.Labels.LimiterTypeDescription });
        }

        /// <inheritdoc/>
        public async Task<bool> TryProcess(Func<Task> operation)
        {
            var operationInfo = this.limit.NotifyStart();

            try
            {
                if (operationInfo.CanProcess)
                {
                    await operation();

                    this.operationsExecuted.Increment();
                }
                else
                {
                    this.operationsLimited.Increment();
                }

                return operationInfo.CanProcess;
            }
            finally
            {
                this.limit.NotifyEnd(operationInfo);
            }
        }

        /// <summary>
        /// Contains information about the metrics provided by the <see cref="SimpleLimiter" />.
        /// </summary>
        public static class Metrics
        {
            /// <summary>
            /// The labels used in the metrics.
            /// </summary>
            public static class Labels
            {
                /// <summary>
                /// The name of the limiter type label.
                /// </summary>
                public const string LimiterTypeName = "type";

                /// <summary>
                /// The name of the limiter type description.
                /// </summary>
                public const string LimiterTypeDescription = "SimpleLimiter";
            }

            /// <summary>
            /// The executed count metric.
            /// </summary>
            public static class Executed
            {
                /// <summary>
                /// The name of the metric.
                /// </summary>
                public const string Name = "concurrency_limiter_executed_total";

                /// <summary>
                /// The metric description.
                /// </summary>
                public const string Description = "The total number of operations that have been executed (i.e. not limited)";
            }

            /// <summary>
            /// The limited count metric.
            /// </summary>
            public static class Limited
            {
                /// <summary>
                /// The name of the metric.
                /// </summary>
                public const string Name = "concurrency_limiter_limited_total";

                /// <summary>
                /// The description of the metric.
                /// </summary>
                public const string Description = "The total number of operations that have been limited";
            }
        }
    }
}