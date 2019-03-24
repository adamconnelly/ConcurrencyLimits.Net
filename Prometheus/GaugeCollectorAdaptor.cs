namespace ConcurrencyLimits.Net.Prometheus
{
    using ConcurrencyLimits.Net.Core;
    using Prometheus;

    /// <summary>
    /// Wraps a prometheus gauge to set the labels.
    /// </summary>
    public class GaugeCollectorAdaptor : ICollector<IGauge>
    {
        private readonly global::Prometheus.Gauge gauge;

        /// <summary>
        /// Initializes a new instance of the <see cref="GaugeCollectorAdaptor" /> class.
        /// </summary>
        /// <param name="gauge">The prometheus gauge.</param>
        public GaugeCollectorAdaptor(global::Prometheus.Gauge gauge)
        {
            this.gauge = gauge;
        }

        /// <summary>
        /// Creates a new gauge with the specified label values.
        /// </summary>
        /// <param name="labelValues">The label values.</param>
        /// <returns>The gauge.</returns>
        public IGauge WithLabels(string[] labelValues)
        {
            return new GaugeAdaptor(this.gauge.WithLabels(labelValues));
        }
    }
}