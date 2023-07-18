using EventBus.Base.Abstraction;
using EventBus.Base.Events;
using Microsoft.Extensions.Logging;
using PaymentService.Api.IntegrationEvents.Events;

namespace PaymentService.Api.IntegrationEvents.EventsHandlers
{
    public class OrderPaymentSuccessIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSuccessIntegrationEvent>
    { 
        private readonly ILogger<OrderPaymentSuccessIntegrationEventHandler> _logger;

        public OrderPaymentSuccessIntegrationEventHandler(  ILogger<OrderPaymentSuccessIntegrationEventHandler> logger)
        {
           
            _logger = logger;
        }



        public Task Handle(OrderPaymentSuccessIntegrationEvent @event)
        {
            // Send Fail Notification (sms email push vs.)
            _logger.LogInformation($"Order Payment Success with OrderId: {@event.OrderId}");
            return Task.CompletedTask;
        }
    }
}
 