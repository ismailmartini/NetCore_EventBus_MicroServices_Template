using EventBus.Base.Abstraction;
using PaymentService.Api;
using PaymentService.Api.IntegrationEvents.Events;
using PaymentService.Api.IntegrationEvents.EventsHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//service registiration
builder.Services.AddEventBusService();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();


 

 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
