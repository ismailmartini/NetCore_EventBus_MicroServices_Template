using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.UnitTest.Events.EventHandlers;
using EventBus.UnitTest.Events.Events;
using EventBus_Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EventBus.UnitTest
{
    public class Tests
    {
        private ServiceCollection services;

        public Tests()
        {
            
        }
        [SetUp]
        public void Setup()
        {
            services = new ServiceCollection();
            services.AddLogging(configure => configure.AddConsole());
        }

        [Test]
        public void subscribe_event_on_rabbitmq_test()
        {
            services.AddSingleton<IEventBus>(sp =>
            {
                
                return EventBusFactory.Create(GetRabbitMQConfig(), sp);
            });


            var sp = services.BuildServiceProvider();


            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

            eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        }
        [Test]
        public void subscribe_event_on_azure_test()
        {
            services.AddSingleton<IEventBus>(sp =>
            { 
                return EventBusFactory.Create(GetAzureConfig(), sp);
            });


            var sp = services.BuildServiceProvider();


            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

            eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        }




        [Test]
        public void send_message_to_rabbitMQ()
        {
            services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetRabbitMQConfig(), sp);
            });


            var sp = services.BuildServiceProvider();


            var eventBus = sp.GetRequiredService<IEventBus>();
            eventBus.Publish(new OrderCreatedIntegrationEvent(1));
        }
        [Test]
        public void send_message_to_azure()
        {
            services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetAzureConfig(), sp);
            });


            var sp = services.BuildServiceProvider();


            var eventBus = sp.GetRequiredService<IEventBus>();
            eventBus.Publish(new OrderCreatedIntegrationEvent(1));
        }


        [Test]
        public void consume_ordercreated_From_rabbitmq_test()
        {
            services.AddSingleton<IEventBus>(sp =>
            {

                return EventBusFactory.Create(GetRabbitMQConfig(), sp);
            });


            var sp = services.BuildServiceProvider();


            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        }



        [Test]
        public void consume_ordercreated_From_azure_test()
        {
            services.AddSingleton<IEventBus>(sp =>
            {

                return EventBusFactory.Create(GetAzureConfig(), sp);
            });


            var sp = services.BuildServiceProvider();


            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        }


        private EventBusConfig GetAzureConfig()
        {
            return new EventBusConfig()
            {

                SubscriberClientAppName = "EventBus.UnitTest",
                DefaultTopicName = "E-Commerce-Microservices-TopicName",
                EventBusType = EventBusType.AzureServiceBus,
                EventNameSuffix = "IntegrationEvent",
                EventBusConnectonString = "azure conf string"


            };


        }


        private EventBusConfig GetRabbitMQConfig()
        {
            return new EventBusConfig()
            {

                   SubscriberClientAppName = "EventBus.UnitTest",
                    DefaultTopicName = "E-Commerce-Microservices-TopicName",
                    EventBusType = EventBusType.RabbitMQ,
                    EventNameSuffix = "IntegrationEvent",
                    //Connection = new ConnectionFactory()
                    //{
                    //    HostName="localhost",
                    //    Port=5672,
                    //    UserName="admin",
                    //    Password="1q2w3e4r"
                    //}


            };


        }

    }



}