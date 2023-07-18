using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus_Factory;
using PaymentService.Api.IntegrationEvents.Events;
using PaymentService.Api.IntegrationEvents.EventsHandlers;
using RabbitMQ.Client;

namespace PaymentService.Api
{
    public  static class ServicesRegistiration
    {
         
        public static void AddEventBusService(this IServiceCollection serviceCollection)
        {
            IConfiguration configuration = serviceCollection.BuildServiceProvider().GetService<IConfiguration>();
            serviceCollection.AddTransient<OrderStartedIntegrationEventHandler>();
            serviceCollection.AddLogging(configure=>   configure.AddConsole()  );
            serviceCollection.AddSingleton<IEventBus>(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = configuration.GetValue<int>("ConnectionRetryCount"),
                    EventNameSuffix = configuration.GetValue<string>("EventNameSuffix"),
                    SubscriberClientAppName = configuration.GetValue<string>("SubscriberClientAppName"),
                   
                    EventBusType = EventBusType.RabbitMQ
                };
                
                return EventBusFactory.Create(config, sp);
            });



           

        }
    }
}
  