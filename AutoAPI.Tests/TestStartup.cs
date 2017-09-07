using AutoAPI.Infrastructure.Caching;
using AutoAPI.Models.Context;
using AutoAPI.Models.Vehicles.Impl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutoAPI.Tests
{
    public class TestStartup: Startup
    {
        public TestStartup(IHostingEnvironment env, ILoggerFactory loggerFactory) : base(env, loggerFactory)
        {
            
        }
        
        public void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddSingleton<ICache, MemCache>();
            services.AddSingleton<IContext, EContext>();
            services.AddSingleton<IVehicle, EVehicle>();
        }
    }
}