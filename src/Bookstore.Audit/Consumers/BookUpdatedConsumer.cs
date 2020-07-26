using System.Threading;
using System.Threading.Tasks;
using Bookstore.Contracts;
using Bookstore.Messaging;

namespace Bookstore.Audit.Consumers
{
    public class BookUpdatedConsumer : ITypedConsumer<BookUpdated>
    {
        private readonly BookstoreAuditContext _dbContext;

        public BookUpdatedConsumer(BookstoreAuditContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ConsumeAsync(BookUpdated message, CancellationToken cancellationToken)
        {
            var userAction = new UserAction
            {
                BookId = message.Id,
                UserId = message.UserId,
                ActionType = ActionType.BookUpdated
            };
            await _dbContext.AddAsync(userAction, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}