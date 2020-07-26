using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ActiveMQ.Artemis.Client;
using ActiveMQ.Artemis.Client.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Bookstore.Messaging
{
    public static class ActiveMqExtensions
    {
        public static IActiveMqBuilder AddTypedConsumer<TMessage, TConsumer>(this IActiveMqBuilder builder, RoutingType routingType) where TConsumer : class, ITypedConsumer<TMessage>
        {
            builder.Services.AddScoped<TConsumer>();
            builder.AddConsumer(typeof(TMessage).Name, routingType,  HandleMessage<TConsumer, TMessage>);
            return builder;
        }
        
        public static IActiveMqBuilder AddTypedConsumer<TMessage, TConsumer>(this IActiveMqBuilder builder, RoutingType routingType, string queue) where TConsumer : class, ITypedConsumer<TMessage>
        {
            builder.Services.AddScoped<TConsumer>();
            var address = typeof(TMessage).Name;
            var queueName = $"{address}/{queue}";
            builder.AddConsumer(address, routingType, queueName, HandleMessage<TConsumer, TMessage>);
            return builder;
        }

        private static async Task HandleMessage<TConsumer, TMessage>(Message message, IConsumer consumer, CancellationToken token, IServiceProvider serviceProvider) where TConsumer : class, ITypedConsumer<TMessage>
        {
            using var scope = serviceProvider.CreateScope();
            var msg = JsonSerializer.Deserialize<TMessage>(message.GetBody<string>());
            var typedConsumer = scope.ServiceProvider.GetService<TConsumer>();
            await typedConsumer.ConsumeAsync(msg, token);
            await consumer.AcceptAsync(message);
        }
    }
}