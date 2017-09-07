using System.Linq;
using AutoAPI.Models.Context;
using AutoAPI.Models.Vehicles.Model;

namespace AutoAPI.Models.Vehicles.Data
{
    public static  class EVehicleInit 
    {
        
       

        public static void Initialize(EContext context)
        {
          
            
            context.Database.EnsureCreated();

            //already db has been seeded before, no need to continue
            if (context.Vehicles.Any())
                return; 

            //seed db
            var vehicles = new Vehicle[]
            {
                new Vehicle {Year = 1980, Make = "Explorer", Model = "Ford"},
                new Vehicle {Year = 2005, Make = "Taho", Model = "Chevy"},
                new Vehicle {Year = 2010, Make = "Edge", Model = "Ford"},
                new Vehicle {Year = 2015, Make = "Evoque", Model = "Land Rover"}
            };


            foreach (var vehicle in vehicles)
            {
                context.Vehicles.Add(vehicle);
            }
            
            context.SaveChanges();
             
        }
    }
}