using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoAPI.Filters;
using AutoAPI.Infrastructure;
using AutoAPI.Infrastructure.Caching;
using AutoAPI.Infrastructure.Configurations;
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
        private ILoggerFactory LoggerFactory;
        
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            
            LoggerFactory = loggerFactory;
            
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
          

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Configuration DI, we can use either IConfiguration or AppSettings class  
            services.AddOptions();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSingleton<IConfiguration>(Configuration);
            
                
            //hook entity framework's db context for use to store data along with connection string.
            //hooking up entity framework strategy to be used inside application 
            services.AddDbContext<EContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Auto_EntityFramework"))); 
            services.AddTransient<IVehicle, EVehicle>();
            
            
            //hook mongo context as a singleton object inside the application  (only one instance per app)
            services.AddSingleton<MContext>(new MContext(Configuration));
            //services.AddTransient<IVehicle, MVehicle>();


            //caching strategy 
            //memcached
            var mcd = new MemCache(Configuration, LoggerFactory);
            //redis
            //var mcd = new RedisCache(Configuration, LoggerFactory);
            services.AddSingleton<ICache>(mcd);
            
            
            // Add mvc framework with a global filter for global validation.
            services.AddMvc(options =>
            {
                //global filter for validating model 
                options.Filters.Add(new ValidationModelAttribute());
            });
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