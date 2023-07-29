using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Features.Command.CreateOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace OrderService.Application
{
    public static class ServiceRegistration
    {


        public static void AddApplicationRegistiration(this IServiceCollection collection)
        {
            var assm=Assembly.GetExecutingAssembly();

            //collection.AddMediatR(typeof(CreateOrderCommandHandler));
            collection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));


            //collection.AddMediatR(assm);
            collection.AddAutoMapper(assm);
        }

    }
}
