namespace ConcurrencyLimits.Net.AspNetCore
{
    using System.Net;
    using System.Threading.Tasks;
    using ConcurrencyLimits.Net.Core;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// An ASP.NET Core middleware that enforces concurrency limits.
    /// </summary>
    public class LimiterMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILimiter limiter;
        private readonly ILogger<LimiterMiddleware> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LimiterMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next part of the request pipeline.</param>
        /// <param name="limiter">The limiter to use.</param>
        /// <param name="logger">The logger.</param>
        public LimiterMiddleware(RequestDelegate next, ILimiter limiter, ILogger<LimiterMiddleware> logger)
        {
            this.next = next;
            this.limiter = limiter;
            this.logger = logger;
        }

        /// <summary>
        /// Performs a stage in the request pipeline.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>The task continuation.</returns>
        public async Task Invoke(HttpContext context)
        {
            if (!(await this.limiter.TryProcess(() => this.next(context))))
            {
                this.logger.LogError("Limit exceeded. Request has been blocked.");

                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                await context.Response.WriteAsync("Too many requests");
            }
        }
    }
}