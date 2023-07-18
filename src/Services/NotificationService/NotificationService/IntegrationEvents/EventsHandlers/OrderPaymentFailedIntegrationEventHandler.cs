using EventBus.Base.Abstraction;
using EventBus.Base.Events;
using Microsoft.Extensions.Logging;
using PaymentService.Api.IntegrationEvents.Events;

namespace PaymentService.Api.IntegrationEvents.EventsHandlers
{
    public class OrderPaymentFailedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
    { 
        private readonly ILogger<OrderPaymentFailedIntegrationEventHandler> _logger;

        public OrderPaymentFailedIntegrationEventHandler(  ILogger<OrderPaymentFailedIntegrationEventHandler> logger)
        {
           
            _logger = logger;
        }



        public Task Handle(OrderPaymentFailedIntegrationEvent @event)
        {
            // Send Fail Notification (sms email push vs.)
            _logger.LogInformation($"Order Payment failed with OrderId: {@event.OrderId}, ErrorMessage: {@event.ErrorMessage}");
            return Task.CompletedTask;
        }
    }
}
 