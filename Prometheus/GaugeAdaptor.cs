namespace ConcurrencyLimits.Net.Prometheus
{
    using ConcurrencyLimits.Net.Core;

    /// <summary>
    /// Wraps a prometheus gauge.
    /// </summary>
    public class GaugeAdaptor : IGauge
    {
        private readonly global::Prometheus.IGauge gauge;

        /// <summary>
        /// Initializes a new instance of the <see cref="GaugeAdaptor" /> class.
        /// </summary>
        /// <param name="gauge">The gauge.</param>
        public GaugeAdaptor(global::Prometheus.IGauge gauge)
        {
            this.gauge = gauge;
        }

        /// <summary>
        /// Sets the value of the gauge.
        /// </summary>
        /// <param name="value">The new value of the gauge.</param>
        public void Set(double value)
        {
            this.gauge.Set(value);
        }
    }
}