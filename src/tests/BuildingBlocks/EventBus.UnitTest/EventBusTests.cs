using EventBus.Base.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventBus_Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBus.Base;
using RabbitMQ.Client;
using EventBus.UnitTest.Events.Events;
using EventBus.UnitTest.Events.EventHandlers;

namespace EventBus.UnitTest
{
    public class EventBusTest
    {
        private ServiceCollection services;

        public EventBusTest()
        {
            services = new ServiceCollection();
            services.AddLogging(configure=>configure.AddConsole());

        }


        [TestMethod]
        public void subscribe_event_on_rabbitmq_test()
        {

            services.AddSingleton<IEventBus>(sp =>
            {
                EventBusConfig config = new()
                {
                    
                    SubscriberClientAppName="EventBus.UnitTest",
                    DefaultTopicName="E-Commerce-Microservices-TopicName",
                    EventBusType=EventBusType.RabbitMQ,
                    EventNameSuffix="IntegrationEvent",
                    //Connection = new ConnectionFactory()
                    //{
                    //    HostName="localhost",
                    //    Port=5672,
                    //    UserName="guest",
                    //    Password="guest"
                    //}
                    //defualt ayarlar


                };
                return EventBusFactory.Create(config, sp);
            });


            var sp = services.BuildServiceProvider();


            var eventBus=sp.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

            eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

        }
    }

    
}
