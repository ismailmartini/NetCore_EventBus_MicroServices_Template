using EventBus.Base.Abstraction;
using EventBus.Base.Events;
using PaymentService.Api.IntegrationEvents.Events;

namespace PaymentService.Api.IntegrationEvents.EventsHandlers
{
    public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly IConfiguration _configuration;   
        private readonly IEventBus _eventBus;
        private readonly ILogger<OrderStartedIntegrationEventHandler> _logger;

        public OrderStartedIntegrationEventHandler(IConfiguration configuration,IEventBus eventBus,ILogger<OrderStartedIntegrationEventHandler> logger)
        {
            _configuration = configuration;
            _eventBus = eventBus;
            _logger = logger;
        }



        public Task Handle(OrderStartedIntegrationEvent @event)
        {
            string keyword = "PaymentSuccess";
            bool paymentSuccessFlag = _configuration.GetValue<bool>(keyword);

            //yeni bir event yarat ve publish et
            IntegrationEvent paymentEvent = paymentSuccessFlag
            ? new OrderPaymentSuccessIntegrationEvent(@event.OrderId)
            : new OrderPaymentFailedIntegrationEvent(@event.OrderId,"This is a Fake error message");

            _logger.LogInformation($"OrderCreatedIntegrationEventHandler in PaymentService is fired with PaymentSuccess : {paymentSuccessFlag}, OrderId :{@event.OrderId}");
            //sonuca göre event'i publish et
            _eventBus.Publish(paymentEvent);
            return Task.CompletedTask;
        }
    }
}
 