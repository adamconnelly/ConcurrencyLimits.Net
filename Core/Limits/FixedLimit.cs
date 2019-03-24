namespace ConcurrencyLimits.Net.Core.Limits
{
    using System;
    using System.Threading;

    /// <summary>
    /// An implementation of a fixed limit (i.e. a hard limit that doesn't change over time).
    /// </summary>
    public class FixedLimit : ILimit
    {
        private readonly int limit;
        private readonly IGauge currentOperations;
        private int inFlight;

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedLimit" /> class.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <param name="metricsRegistry">The metrics registry.</param>
        public FixedLimit(int limit, IMetricsRegistry metricsRegistry)
        {
            this.limit = limit;

            var maxOperations = metricsRegistry.CreateGauge(
                Metrics.MaxOperations.Name,
                Metrics.MaxOperations.Description,
                new[] { Metrics.Labels.LimitTypeName });
            maxOperations.WithLabels(new[] { Metrics.Labels.LimitTypeValue }).Set(limit);

            this.currentOperations = metricsRegistry.CreateGauge(
                    Metrics.CurrentOperations.Name,
                    Metrics.CurrentOperations.Description,
                    new[] { Metrics.Labels.LimitTypeName })
                .WithLabels(new[] { Metrics.Labels.LimitTypeValue });
        }

        /// <summary>
        /// Gets the limit.
        /// </summary>
        public int Limit
        {
            get { return this.limit; }
        }

        /// <summary>
        /// Notifies the limit of the intention to start a new operation.
        /// </summary>
        /// <returns>
        /// Details about the operation, including whether it can proceed or not based on the limit.
        /// </returns>
        public OperationInfo NotifyStart()
        {
            var current = Interlocked.Increment(ref this.inFlight);

            this.currentOperations.Set(current);

            return new OperationInfo(DateTime.Now, current <= this.limit);
        }

        /// <summary>
        /// Notifies the limit that a particular operation has completed.
        /// </summary>
        /// <param name="info">The information about the operation.</param>
        public void NotifyEnd(OperationInfo info)
        {
            var current = Interlocked.Decrement(ref this.inFlight);
            this.currentOperations.Set(current);
        }

        /// <summary>
        /// Contains constants for the metrics exported via the limit.
        /// </summary>
        public static class Metrics
        {
            /// <summary>
            /// Contains constants for the labels.
            /// </summary>
            public static class Labels
            {
                /// <summary>
                /// The name of the label used to specify the type of limit employed.
                /// </summary>
                public const string LimitTypeName = "type";

                /// <summary>
                /// The value of the limit type label.
                /// </summary>
                public const string LimitTypeValue = "FixedLimit";
            }

            /// <summary>
            /// Contains constants for the max operations metric.
            /// </summary>
            public static class MaxOperations
            {
                /// <summary>
                /// The name used for the max operations metric.
                /// </summary>
                public const string Name = "concurrency_limit_max_operations";

                /// <summary>
                /// The description of the max operations metric.
                /// </summary>
                public const string Description = "The current max number of operations that can be processed concurrently";
            }

            /// <summary>
            /// Contains constants for the current operations metric.
            /// </summary>
            public static class CurrentOperations
            {
                /// <summary>
                /// The name of the metric.
                /// </summary>
                public const string Name = "concurrency_limit_current_operations";

                /// <summary>
                /// The description of the metric.
                /// </summary>
                public const string Description = "The current number of requests concurrently executing";
            }
        }
    }
}