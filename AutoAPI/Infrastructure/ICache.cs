namespace AutoAPI.Infrastructure
{
    public interface ICache
    {
        void Store(string key, object value);
        object Get(string key);
    }
}