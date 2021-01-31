using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ActiveMQ.Artemis.Client;
using ActiveMQ.Artemis.Client.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bookstore.Messaging
{
    public static class ActiveMqExtensions
    {
        public static IActiveMqBuilder AddTypedConsumer<TMessage, TConsumer>(this IActiveMqBuilder builder, RoutingType routingType)
            where TConsumer : class, ITypedConsumer<TMessage>
        {
            builder.Services.TryAddScoped<TConsumer>();
            builder.AddConsumer(typeof(TMessage).Name, routingType, HandleMessage<TMessage, TConsumer>);
            return builder;
        }

        private static async Task HandleMessage<TMessage, TConsumer>(Message message, IConsumer consumer, IServiceProvider serviceProvider, CancellationToken token)
            where TConsumer : class, ITypedConsumer<TMessage>
        {
            var msg = JsonSerializer.Deserialize<TMessage>(message.GetBody<string>());
            using var scope = serviceProvider.CreateScope();
            var typedConsumer = scope.ServiceProvider.GetService<TConsumer>();
            await typedConsumer.ConsumeAsync(msg, token);
            await consumer.AcceptAsync(message);
        }

        public static IActiveMqBuilder AddTypedConsumer<TMessage, TConsumer>(this IActiveMqBuilder builder, RoutingType routingType, string queue)
            where TConsumer : class, ITypedConsumer<TMessage>
        {
            builder.Services.TryAddScoped<TConsumer>();
            var address = typeof(TMessage).Name;
            var queueName = $"{address}/{queue}";
            builder.AddConsumer(address, routingType, queueName, HandleMessage<TMessage, TConsumer>);
            return builder;
        }
    }
}