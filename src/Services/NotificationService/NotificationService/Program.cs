﻿using EventBusBase;
using EventBusBase.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.IntegrationEvents.EventHandlers;
using NotificationService.IntegrationEvents.Events;

namespace NotificationService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();

            ConfigureServices(services);

            var sp = services.BuildServiceProvider();

            IEventBus eventBus = sp.GetRequiredService<IEventBus>();
            eventBus.Subscribe<OrderPaymentSuccessIntegrationEvent, OrderPaymentSuccessIntegrationEventHandler>();
            eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();

            Console.WriteLine("Application is running...");
            Console.ReadLine();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole());

            services.AddTransient<OrderPaymentSuccessIntegrationEventHandler>();
            services.AddTransient<OrderPaymentFailedIntegrationEventHandler>();

            services.AddSingleton<IEventBus>(sp =>

                EventBusFactory.EventBusFactory.Create(new()
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = "IntegrationEvent",
                    SubscriberClientAppName = "NotificationService",
                    EventBusType = EventBusType.RabbitMQ
                }, sp)

            );
        }
    }
}
