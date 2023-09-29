using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public class RedisCachingService: ICachingService
    {
        private readonly ConnectionMultiplexer redisConnection;
        private readonly IDatabase database;
        private readonly int expiration;

        public RedisCachingService(IConfiguration config)
        {
            try
            {
                var connectionString = config.GetSection("Redis:Uri")?.Value;
                expiration = int.Parse(config.GetSection("Redis:CacheExpiration")?.Value);
                redisConnection = ConnectionMultiplexer.Connect(connectionString);
                database = redisConnection.GetDatabase();
            }
            catch (Exception ex)
            {
                //Log here
            }
        }

        public async Task SetAsync<T>(string key, T value)
        {
            await database.StringSetAsync(key, JsonConvert.SerializeObject(value), TimeSpan.FromMinutes(expiration));
        }

        public async Task<T> TryGetAsync<T>(string key)
        {
            var value = await database.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default;
        }
    }
}
