using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Cache.Services;

public class CacheService(IMemoryCache memoryCache) : ICacheService
{
    public void ClearCache()
    {
        memoryCache.Remove("Matches");
        memoryCache.Remove("Ranking");
    }
}