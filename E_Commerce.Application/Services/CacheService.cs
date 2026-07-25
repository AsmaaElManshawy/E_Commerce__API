using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepo;

        public CacheService(ICacheRepository cacheRepo)
        {
            _cacheRepo = cacheRepo;
        }

        public Task<string?> GetDataAsync(string cacheKey, CancellationToken ct = default)
            => _cacheRepo.GetAsync(cacheKey, ct);

        public async Task SetDataAsync(string cacheKey, object CacheValue, TimeSpan timeToLive = default, CancellationToken ct = default)
        {
            var jsonValue = JsonSerializer.Serialize(CacheValue , new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await _cacheRepo.SetAsync(cacheKey, jsonValue, timeToLive, ct);
        }

    }
}
