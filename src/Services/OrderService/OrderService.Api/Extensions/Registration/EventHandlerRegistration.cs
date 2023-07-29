using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OrderService.Api.IntegrationEvents.EventHandlers;
using System.Text;

namespace OrderService.Api.Extensions.Registration
{
    public static class EventHandlerRegistration
    {
        public static IServiceCollection ConfigureEventHandlers(this IServiceCollection services)
        {
            services.AddTransient<OrderCreatedIntegrationEventHandler> ();
    
            return services;

        }
    }
}
