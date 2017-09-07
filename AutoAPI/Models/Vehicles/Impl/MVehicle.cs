using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoAPI.Models.Context;
using AutoAPI.Models.Vehicles.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AutoAPI.Models.Vehicles.Impl
{
    //Mongo-based vehicle (operations) - mongodb is one of the most popular no sql dbs 
    //also we can have later on some other implementations so this will not affect current code or need any changes just put your new implementation and 
    //and use it 
    //this is also is really helpful in unit-testing operations.
    //we can use Structure Map or Ninject for dependency injections however I implemented in my manual way here .
    public class MVehicle : IVehicle
    {
        private MContext _context;

//        //favor composition over inheritance for extensibility (not tightl coupled)
//        public Vehicle Vehicle { get; set; }


        public MVehicle(MContext context)
        {
            _context = context;
        }
        
        public async Task<bool> Save(Vehicle vehicle)
        {
            try
            {
                var filter = Builders<Vehicle>.Filter.Eq("_id", vehicle.Id);

                var r = await _context.Vehicles.ReplaceOneAsync(filter, vehicle);
                if (r.ModifiedCount > 0)
                    return true;
            }
            catch (Exception ex)
            {
                //there should be some logging
                return false;
            }

            return false;

        }

        public async Task<long> Count()
        {
            return  await _context.Vehicles.CountAsync(new BsonDocument());
        }
        
        public async Task<Vehicle> GetById(string id)
        {

            var filter = Builders<Vehicle>.Filter.Eq("_id", id);
            using (var cursor = await _context.Vehicles.FindAsync(filter))
            {

                if (cursor.MoveNext())
                    return cursor.Current.First();
            }

            return null;
        }


        public async Task<bool> Exists(string id)
        {
            return await _context.Vehicles.Find(_ => _.Id == id).AnyAsync();

        }
        
        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            List<Vehicle> list = new List<Vehicle>();
             
            using (var cursor = await _context.Vehicles.FindAsync(new BsonDocument()))
            {

                while (cursor.MoveNext())
                    list.AddRange(cursor.Current);
            }

            return list;
            
         }

        public async Task<bool> DeleteById(string id)
        {
            try
            {
                var filter = Builders<Vehicle>.Filter.Eq("_id", id);

                var r = await _context.Vehicles.DeleteOneAsync(filter);
                if (r.DeletedCount <= 0)
                    return false;
            }
            catch (Exception ex)
            {
                //there should be some logging mechanism here 
                throw;
            }
            
            return true;
        }
    }
}