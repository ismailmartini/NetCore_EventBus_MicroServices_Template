using BasketService.Api.IntegrationEvents.EventsHandlers;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus_Factory;
 
using RabbitMQ.Client;

namespace BasketService.Api
{
    public  static class EventBusRegistiration
    {
         
        public static void AddEventBusService(this IServiceCollection serviceCollection)
        {
            IConfiguration configuration = serviceCollection.BuildServiceProvider().GetService<IConfiguration>();
            serviceCollection.AddTransient<OrderCreatedIntegrationEventHandler>();
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
  