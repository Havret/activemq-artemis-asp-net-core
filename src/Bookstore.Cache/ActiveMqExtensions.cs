using System.Text.Json;
using ActiveMQ.Artemis.Client;
using ActiveMQ.Artemis.Client.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Bookstore.Cache
{
    public static class ActiveMqExtensions
    {
        public static IActiveMqBuilder AddTypedConsumer<TConsumer, TMessage>(this IActiveMqBuilder builder, RoutingType routingType) where TConsumer : class, ITypedConsumer<TMessage>
        {
            builder.Services.AddScoped<TConsumer>();
            builder.AddConsumer(typeof(TMessage).Name, routingType, async (message, consumer, token, serviceProvider) =>
            {
                using var scope = serviceProvider.CreateScope();
                var msg = JsonSerializer.Deserialize<TMessage>(message.GetBody<string>());
                var typedConsumer = scope.ServiceProvider.GetService<TConsumer>();
                await typedConsumer.ConsumeAsync(msg, token);
                await consumer.AcceptAsync(message);
            });
            return builder;
        }
    }
}