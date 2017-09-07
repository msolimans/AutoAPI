using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AutoAPI.Models.Vehicles.Model
{
    //this is the only model for vehicle which is gonna be used in any contract based implementation 
    public class Vehicle
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
}