using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Cache
{
    public class BookCache
    {
        private readonly ConcurrentDictionary<Guid, BookCacheEntry> _cache = new ConcurrentDictionary<Guid, BookCacheEntry>();
        
        public Task AddAsync(BookCacheEntry cacheEntry, CancellationToken cancellationToken)
        {
            _cache.TryAdd(cacheEntry.Id, cacheEntry);
            return Task.CompletedTask;
        }

        public Task<BookCacheEntry> GetAsync(Guid id)
        {
            if (_cache.TryGetValue(id, out var cacheEntry))
                return Task.FromResult(cacheEntry);
            else
                return Task.FromResult((BookCacheEntry) null);
        }
    }
}