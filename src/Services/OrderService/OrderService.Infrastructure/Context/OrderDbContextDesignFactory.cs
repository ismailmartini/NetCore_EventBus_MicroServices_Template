using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
 

namespace OrderService.Infrastructure.Context
{
    public class OrderDbContextDesignFactory : IDesignTimeDbContextFactory<OrderDbContext>
    {
        public OrderDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>()
                .UseSqlServer("Data Source=DESKTOP-8S3OCJ3\\SQLEXPRESS;Initial Catalog=catalog;Persist Security Info=True;User ID=sa;Password=1");

            return new OrderDbContext(optionsBuilder.Options,new NoMediator());
        }

        class NoMediator : IMediator
        {
            public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
            {
                return (IAsyncEnumerable<TResponse>)request;
                 
            }

            public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
            {
                return (IAsyncEnumerable<object>)request;
            }

            public Task Publish(object notification, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
              
            }

            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
            {
                return Task.FromResult<TResponse>(default);
            }

            public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
            {
                return Task.FromResult<TRequest>(default);
                
            }

            public Task<object?> Send(object request, CancellationToken cancellationToken = default)
            {
                return Task.FromResult<object>(default);
                 
            }
        }
    }
}
