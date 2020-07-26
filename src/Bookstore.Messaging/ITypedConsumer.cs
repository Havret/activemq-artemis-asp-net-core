using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Messaging
{
    public interface ITypedConsumer<in T>
    {
        public Task ConsumeAsync(T message, CancellationToken cancellationToken);
    }
}