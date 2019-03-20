using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConcurrencyLimits.Net.AspNetCore;
using ConcurrencyLimits.Net.Core;
using ConcurrencyLimits.Net.Core.Limiters;
using ConcurrencyLimits.Net.Core.Limits;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNetCore
{
    public class Startup
    {
        private IServiceCollection services;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.services = services;
            services.AddSingleton(typeof(ILimiter), new SimpleLimiter(new FixedLimit(5)));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseConcurrencyLimit(services);

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
