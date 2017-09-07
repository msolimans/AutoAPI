using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoAPI.Models;
using AutoAPI.Models.Context;
using AutoAPI.Models.Vehicles.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    //entity framework context
                    var context = services.GetRequiredService<EContext>();
                    //initialize sql server here 
                    EVehicleInit.Initialize(context);

                    //mongo context
                    var mcontext = services.GetService<MContext>();
                    //initialize mongodb with data 
                    MVehicleInit.Initiatlize(mcontext); 
                    
                    
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(new EventId(10), ex, "Error while seeding database");
                }
            }
            
            host.Run();    
        }
    }
}