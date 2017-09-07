using System;
using AutoAPI.Models.Vehicles.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AutoAPI.Models.Context
{
    public class MContext : IContext
    {
        public const string CONNECTION_STRING_NAME = "Auto_Mongo";
        public const string DATABASE_NAME = "auto";
        public const string VEHICLE_COLLECTION_NAME = "vehicle";

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;


        private IConfiguration Configuration { get; set; }

        public MContext(IConfiguration configuration)
        {
            Configuration = configuration;


            //singleton pattern, responsible for initiation 

            if (_client == null)
            {
                var connectionString = Configuration.GetConnectionString(CONNECTION_STRING_NAME);
                _client = new MongoClient(connectionString);
            }

            if (_database == null)
                _database = _client.GetDatabase(DATABASE_NAME);
        }

        public IMongoClient Client => _client;


        public IMongoCollection<Vehicle> Vehicles => _database.GetCollection<Vehicle>(VEHICLE_COLLECTION_NAME);
    }
}