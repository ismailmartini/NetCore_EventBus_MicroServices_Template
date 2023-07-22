using BasketService.Api;
using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Extensions;
using BasketService.Api.Infrastructure.Repository;
using BasketService.Api.IntegrationEvents.Events;
using BasketService.Api.IntegrationEvents.EventsHandlers;
using EventBus.Base.Abstraction;
 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureConsul();
builder.Services.ConfigureAuth(); 
builder.Services.ConfigureRedis();
builder.Services.AddEventBusService();
builder.Services.AddHttpContextAccessor();  
builder.Services.AddScoped<IBasketRepository,RedisBasketRepository>();
builder.Services.AddTransient<IIdentityService, BasketService.Api.Core.Application.Services.IdentityService>();


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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();





app.Start();
app.RegisterWithConsul(builder.Services);
app.WaitForShutdown();