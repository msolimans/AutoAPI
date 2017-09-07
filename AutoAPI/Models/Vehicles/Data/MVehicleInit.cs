using AutoAPI.Models.Context;
using AutoAPI.Models.Vehicles.Model;
using Microsoft.AspNetCore.Hosting.Internal;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AutoAPI.Models.Vehicles.Data
{
    public  static class MVehicleInit
    {
        public static void Initiatlize(MContext context)
        {
          
            
            var count = context.Vehicles.Count(new BsonDocument());
            if(count>0)
                return;
           
            context.Vehicles.InsertMany(new Vehicle[]
            {
                new Vehicle {Year = 1980, Make = "Explorer", Model = "Ford"},
                new Vehicle {Year = 2005, Make = "Taho", Model = "Chevy"},
                new Vehicle {Year = 2010, Make = "Edge", Model = "Ford"},
                new Vehicle {Year = 2015, Make = "Evoque", Model = "Land Rover"}
            });
            
            
           


        }
        
        
    }
}