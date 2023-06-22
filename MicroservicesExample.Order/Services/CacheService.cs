using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MicroservicesExample.Order.Services
{
    public class CacheService : ICacheService
    {
        private IDatabase _db;
        private readonly IConnectionMultiplexer _redis;

        public CacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase(); 
        }
        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
            return isSet;
        }
        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }
    }
}
