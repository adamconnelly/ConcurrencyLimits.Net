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
        private readonly ILimiter limiter;
        private readonly ILogger<LimiterMiddleware> logger;

        public LimiterMiddleware(RequestDelegate next, ILimiter limiter, ILogger<LimiterMiddleware> logger)
        {
            this.next = next;
            this.limiter = limiter;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var (canProceed, listener) = this.limiter.Acquire();
            if (canProceed)
            {
                await this.next(context);
                
                listener.Success();
            }
            else
            {
                logger.LogError("Limit exceeded. Request has been blocked. Current state '{LimiterState}'", this.limiter.StateDescription);

                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                await context.Response.WriteAsync("Too many requests");
            }
        }
    }
}