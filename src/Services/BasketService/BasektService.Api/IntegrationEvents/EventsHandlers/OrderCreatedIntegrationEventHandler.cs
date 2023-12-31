﻿using BasketService.Api.Core.Application.Repository;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base.Abstraction;
using EventBus.Base.Events;
using Microsoft.VisualBasic;
using System.Threading.Channels;

namespace BasketService.Api.IntegrationEvents.EventsHandlers
{
    public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {



        private readonly IBasketRepository _repository;
        private readonly ILogger<OrderCreatedIntegrationEvent> _logger;

        public OrderCreatedIntegrationEventHandler(IBasketRepository repository, ILogger<OrderCreatedIntegrationEvent> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task Handle(OrderCreatedIntegrationEvent @event)
        {
            _logger.LogInformation("--- Handling integration event: {IntegrationEventId} at Basket Service.Api - ({@IntegrationEvent})", @event.Id);

            await _repository.DeleteBasketAsync(@event.UserId.ToString());
        }



            }
}
