using System.Threading.Tasks;

namespace AutoAPI.Infrastructure.Caching
{
    public interface ICache
    {
        void Store(string key, object value);
        object Get(string key);
        void Remove(string key);
    }
}