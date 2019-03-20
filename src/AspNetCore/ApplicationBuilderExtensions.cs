using System;
using ConcurrencyLimits.Net.Core;
using ConcurrencyLimits.Net.Core.Limiters;
using ConcurrencyLimits.Net.Core.Limits;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ConcurrencyLimits.Net.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseConcurrencyLimit(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LimiterMiddleware>();
        }
    }
}
