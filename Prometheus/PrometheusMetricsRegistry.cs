namespace ConcurrencyLimits.Net.Prometheus
{
    using ConcurrencyLimits.Net.Core;
    using global::Prometheus;

    /// <summary>
    /// A metrics registry that exports prometheus metrics.
    /// </summary>
    public class PrometheusMetricsRegistry : IMetricsRegistry
    {
        /// <inheritdoc/>
        public ICollector<Core.ICounter> CreateCounter(
            string name, string description, string[] labels)
        {
            return new CounterCollectorAdaptor(
                Metrics.CreateCounter(
                    name, description, new CounterConfiguration { LabelNames = labels }));
        }

        /// <inheritdoc/>
        public ICollector<Core.IGauge> CreateGauge(
            string name, string description, string[] labels)
        {
            return new GaugeCollectorAdaptor(
                Metrics.CreateGauge(
                    name, description, new GaugeConfiguration { LabelNames = labels }));
        }
    }
}
