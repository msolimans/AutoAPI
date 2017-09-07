using Newtonsoft.Json;

namespace AutoAPI.Infrastructure.Serializations
{
    public static class JsonEx
    {
        public static string Serialize(object value)
        {
            var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};

            return JsonConvert.SerializeObject(value, typeof(object), settings);

        }

        public static object Deserialize(string value)
        {
            return JsonConvert.DeserializeObject(value,
                new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto});

        }
    }
}