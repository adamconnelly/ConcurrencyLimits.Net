# Metrics

The library will provide metrics support out the box to make it easy to monitor
how it performs, and alert when there are issues. The goal will be to provide
a pluggable interface to allow various metrics systems to be connected, but to
provide a [Prometheus](https://prometheus.io) implementation out the box. The
Prometheus implementation will be provided in a separate library so that we
don't have a Prometheus dependency for people who aren't using it.

## Naming

We'll aim to follow Prometheus [best practices](https://prometheus.io/docs/practices/naming/)
on naming.

All metrics will use an application prefix of `concurrency`.

## API

To begin with, we'll roughly wrap the [Prometheus client library](https://github.com/prometheus-net/prometheus-net)
API. This might change later if it turns out this is difficult to integrate
with other metrics systems.

The main component that will need to be implemented will be `IMetricsRegistry`:

```csharp
public interface IMetricsRegistry
{
    ICollector<ICounter> CreateCounter(
        string name, string description, string[] labels);
    ICollector<IGauge> CreateGauge(string name, string description, string[] labels);
}
```

There will be an out the box `DefaultMetricsRegistry` that basically implements
a no-op version of the metrics, and then a `PrometheusMetricsRegistry` that
implements Prometheus metrics.

The `ICollector` interface allows the label values to be specified to create a
particular time series:

```csharp
public interface ICollector<T>
{
    T WithLabels(string[] labelValues);
}
```

The interface for `ICounter` will look roughly like this:

```csharp
public interface ICounter
{
    void Increment();
}
```

The interface for `IGauge` will look roughly like this:

```csharp
public interface IGauge
{
    void Set(double value);
}
```

## Metrics Provided

### Limit Metrics

We will record the following metrics for Limits:

| Name                                 | Type  | Description                                                             |
|--------------------------------------|-------|-------------------------------------------------------------------------|
| concurrency_limit_max_operations     | Gauge | The current max number of operations that can be processed concurrently |
| concurrency_limit_current_operations | Gauge | The current number of requests concurrently executing                   |

**NOTE:** the `concurrency_limit_current_operations` metric currently can go
above the `concurrency_limit_max_operations` metric value as implemented because
it includes the requests that are currently in-flight that should not be executed.
I'm not sure if this makes sense or not.

Different types of Limits will also provide other metrics. This section can be
updated as more Limit algorithms are implemented.

### Limiter Metrics

We will record the following metrics for Limiters:

| Name                                   | Type    | Description                                                               |
|----------------------------------------|---------|---------------------------------------------------------------------------|
| concurrency_limiter_executed_total     | Counter | The total number of operations that have been executed (i.e. not limited) |
| concurrency_limiter_limited_total      | Counter | The total number of operations that have been limited                     |
