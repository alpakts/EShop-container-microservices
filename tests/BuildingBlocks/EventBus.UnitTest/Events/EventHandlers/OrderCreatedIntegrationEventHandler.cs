using EventBus.UnitTest.Events.Events;
using EventBusBase.Abstraction;

namespace EventBus.UnitTest.Events.EventHandlers
{
    internal class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        public Task Handle(OrderCreatedIntegrationEvent @event)
        {
            Console.WriteLine("Handle method worked with id: " + @event.Id);
            return Task.CompletedTask;
        }
    }
}
