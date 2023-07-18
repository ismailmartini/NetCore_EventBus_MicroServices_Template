
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus_Factory;
 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaymentService.Api.IntegrationEvents.Events;
using PaymentService.Api.IntegrationEvents.EventsHandlers;


ServiceCollection services=new ServiceCollection();
ConfigureServices(services);
var sp=services.BuildServiceProvider();
IEventBus eventBus = sp.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderPaymentSuccessIntegrationEvent, OrderPaymentSuccessIntegrationEventHandler>();
eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();


Console.WriteLine("Application is Running...");
Console.ReadLine();

static void  ConfigureServices(IServiceCollection serviceCollection)
{

    serviceCollection.AddTransient<OrderPaymentFailedIntegrationEventHandler>();
    serviceCollection.AddTransient<OrderPaymentSuccessIntegrationEventHandler>();

    serviceCollection.AddLogging(configure => configure.AddConsole());
    serviceCollection.AddSingleton<IEventBus>(sp =>
    {
        EventBusConfig config = new()
        {
            ConnectionRetryCount = 5,
            EventNameSuffix = "IntegrationEvent",
            SubscriberClientAppName = "NotificationService",
            EventBusType = EventBusType.RabbitMQ
        };

        return EventBusFactory.Create(config, sp);
    });
}