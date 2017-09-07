using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace AutoAPI.Models.Vehicles.Model
{
    //this is the only model for vehicle which is gonna be used in any contract based implementation 
    
    public class Vehicle
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [Range(1950,2050)]
        public int Year { get; set; }
        
        [Required]
        public string Make { get; set; }
        
        [Required]
        public string Model { get; set; }
    }
}    