namespace ConcurrencyLimits.Net.AspNetCore
{
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Contains extension methods for <see cref="IApplicationBuilder" />.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Enables concurrency limits as part of the ASP.NET core pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns>The builder.</returns>
        public static IApplicationBuilder UseConcurrencyLimit(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LimiterMiddleware>();
        }
    }
}
