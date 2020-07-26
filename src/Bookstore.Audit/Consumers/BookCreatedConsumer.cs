using System.Threading;
using System.Threading.Tasks;
using Bookstore.Contracts;
using Bookstore.Messaging;

namespace Bookstore.Audit.Consumers
{
    internal class BookCreatedConsumer : ITypedConsumer<BookCreated>
    {
        private readonly BookstoreAuditContext _dbContext;

        public BookCreatedConsumer(BookstoreAuditContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ConsumeAsync(BookCreated message, CancellationToken cancellationToken)
        {
            var userAction = new UserAction
            {
                BookId = message.Id,
                UserId = message.UserId,
                ActionType = ActionType.BookCreated
            };
            await _dbContext.AddAsync(userAction, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}