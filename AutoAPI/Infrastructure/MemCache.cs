using System;
using System.Net;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AutoAPI.Infrastructure
{
    public class MemCache: ICache
    {
        private IConfiguration _configuration;
        private ILoggerFactory _loggerFactory;
        private MemcachedClient _client;
        
        public MemCache(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
            
            
            MemcachedClientConfiguration mconfig = new MemcachedClientConfiguration(_loggerFactory, new MemcachedClientOptions());
            //configuration can be added to appSettings for mutiple servers here but I just added here staticially for demo purpose only. 
            mconfig.Servers.Add(new System.Net.IPEndPoint(IPAddress.Loopback, 11211));

            mconfig.Protocol = Enyim.Caching.Memcached.MemcachedProtocol.Binary;//faster 

            //authentication can be added later (by supporting username/password)
            //config.Authentication.Parameters["username"] = "username";
            //config.Authentication.Parameters["password"] = "password";
            
            
            _client = new MemcachedClient(_loggerFactory, mconfig); 
            

        }
        
        
        public void Store(string key, object value)
        {
            try
            {
                var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};

                var json = JsonConvert.SerializeObject(value, typeof(object), settings);

                _client.Store(StoreMode.Set, key, json);
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
                dynamic value = _client.Get(key);
                if (value == null)
                    return null;
                value = value.ToString();
                
                

                return JsonConvert.DeserializeObject(value,
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto});
            }
            catch (Exception ex)
            {
                //exception should be logged
                return null;
            }

        }
    }
}