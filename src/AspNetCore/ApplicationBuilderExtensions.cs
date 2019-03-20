namespace ConcurrencyLimits.Net.AspNetCore
{
    using Microsoft.AspNetCore.Builder;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseConcurrencyLimit(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LimiterMiddleware>();
        }
    }
}
