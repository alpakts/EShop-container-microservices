using EventBusBase;
using EventBusBase.Abstraction;
using EventBusFactory;
using PaymentService.Api.IntegrationEvents.Events;
using PaymentService.Api.IntegrationEvents.IntegrationEventHandlers;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(configure => configure.AddConsole());
builder.Services.AddTransient<OrderStartedIntegrationEventHandler>();
builder.Services.AddSingleton<IEventBus>(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetryCount = 10,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "Payment Service",
        Connection = new ConnectionFactory(),
        EventBusType = EventBusType.RabbitMQ,
    };
    return EventBusFactory.EventBusFactory.Create(config, sp);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();
app.Run();
