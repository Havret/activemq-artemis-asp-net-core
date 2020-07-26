using System.Threading;
using System.Threading.Tasks;
using Bookstore.Contracts;
using Bookstore.Messaging;

namespace Bookstore.Cache.Consumers
{
    internal class BookCreatedConsumer : ITypedConsumer<BookCreated>
    {
        private readonly BookCache _bookCache;

        public BookCreatedConsumer(BookCache bookCache)
        {
            _bookCache = bookCache;
        }

        public async Task ConsumeAsync(BookCreated message, CancellationToken cancellationToken)
        {
            await _bookCache.AddOrUpdate(new BookCacheEntry(
                id: message.Id,
                title: message.Title,
                author: message.Author,
                cost: message.Cost), cancellationToken);
        }
    }
}