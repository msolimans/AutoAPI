using System;
using AutoAPI.Infrastructure.Serializations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace AutoAPI.Infrastructure.Caching
{
    public class RedisCache : ICache
    {
        private IConfiguration _configuration;
        private ILoggerFactory _loggerFactory;
        
        private readonly IDatabase _database;


        public RedisCache(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
            var redis = ConnectionMultiplexer.Connect("127.0.0.1");//configuration.GetSection("Caching.redis").Value
            _database = redis.GetDatabase();
            
        }

        public void Store(string key, object value)
        {
            try
            { 
                _database.StringSet(key, JsonEx.Serialize(value));
            }
            catch (Exception ex)
            {
                //log here exception
            }

            
        }

        public object Get(string key)
        {
            try
            {
                dynamic value = _database.StringGet(key);
                
                if (string.IsNullOrEmpty(value))
                    return null;
                
                value = value.ToString();
                
                

                return JsonEx.Deserialize(value);
            }
            catch (Exception ex)
            {
                //exception should be logged
                return null;
            }

        }

        public void Remove(string key)
        {
            _database.KeyDelete(key);
        }
    }
}