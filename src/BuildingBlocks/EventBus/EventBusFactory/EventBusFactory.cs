using EventBus.AzureServiceBus;
using EventBus.RabbitMQ;
using EventBusBase;
using EventBusBase.Abstraction;

namespace EventBusFactory
{
    public static class EventBusFactory
    {
        public static IEventBus Create(EventBusConfig config, IServiceProvider serviceProvider)
        {
            return config.EventBusType switch
            {
                EventBusType.AzureServiceBus => new EventBusServiceBus(config, serviceProvider),
                _ => new EventBusRabbitMQ(config, serviceProvider),
            };
        }
    }
}
