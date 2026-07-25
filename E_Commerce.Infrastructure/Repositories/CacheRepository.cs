using E_Commerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDatabase = StackExchange.Redis.IDatabase;

namespace E_Commerce.Infrastructure.Repositories
{
    public class CacheRepository : ICacheRepository 
    {
        // Database connection
        private readonly IDatabase _database;
        public CacheRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();

        }

        public async Task<string?> GetAsync(string cacheKey, CancellationToken ct = default)
        {
            var value = await _database.StringGetAsync(cacheKey);
            return value.IsNullOrEmpty ? null : value.ToString();
        }

        public Task SetAsync(string cacheKey, string CacheValue, TimeSpan? timeToLive = default, CancellationToken ct = default)
        {
            return _database.StringSetAsync(cacheKey, CacheValue, timeToLive ?? TimeSpan.FromDays(2));
        }
    }
}
