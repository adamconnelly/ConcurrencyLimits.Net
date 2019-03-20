using System;
using System.Net;
using System.Threading.Tasks;
using ConcurrencyLimits.Net.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ConcurrencyLimits.Net.AspNetCore
{
    public class LimiterMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IAlternativeLimiter limiter;
        private readonly ILogger<LimiterMiddleware> logger;

        public LimiterMiddleware(RequestDelegate next, IAlternativeLimiter limiter, ILogger<LimiterMiddleware> logger)
        {
            this.next = next;
            this.limiter = limiter;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!(await this.limiter.TryProcess(() => this.next(context))))
            {
                logger.LogError("Limit exceeded. Request has been blocked.");

                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                await context.Response.WriteAsync("Too many requests");
            }
        }
    }
}