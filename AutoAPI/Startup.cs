using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoAPI.Models;
using AutoAPI.Models.Context;
using AutoAPI.Models.Vehicles.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AutoAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Auto_EntityFramework")));
            

            services.AddSingleton<IConfiguration>(Configuration);
            
            //hook mongo context as a singleton object inside the application  (only one instance per app)
            services.AddSingleton<MContext>(new MContext(Configuration));

            //hooking up entity framework strategy to be used inside application 
            services.AddTransient<IVehicle, EVehicle>();
            
            //in case we need to use mongodb inside the whole appliction, just comment the above line and uncomment the followinf one.
            //services.AddTransient<IVehicle, MVehicle>();
            
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}