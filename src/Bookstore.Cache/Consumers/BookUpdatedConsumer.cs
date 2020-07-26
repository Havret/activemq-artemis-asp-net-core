using System.Threading;
using System.Threading.Tasks;
using Bookstore.Contracts;
using Bookstore.Messaging;

namespace Bookstore.Cache.Consumers
{
    public class BookUpdatedConsumer : ITypedConsumer<BookUpdated>
    {
        private readonly BookCache _bookCache;

        public BookUpdatedConsumer(BookCache bookCache)
        {
            _bookCache = bookCache;
        }
        
        public async Task ConsumeAsync(BookUpdated message, CancellationToken cancellationToken)
        {
            await _bookCache.AddOrUpdate(new BookCacheEntry(
                id: message.Id,
                title: message.Title,
                author: message.Author,
                cost: message.Cost), cancellationToken);
        }
    }
}