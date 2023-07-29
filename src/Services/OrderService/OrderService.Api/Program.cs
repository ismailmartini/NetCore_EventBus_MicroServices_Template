using OrderService.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Context;
using OrderService.Application;
using OrderService.Infrastructure;
using OrderService.Api.Extensions.Registration;
 
using OrderService.Api;
using EventBus.Base.Abstraction;
using OrderService.Api.IntegrationEvents.EventHandlers;
using OrderService.Api.IntegrationEvents.Events;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddApplicationRegistiration();
builder.Services.AddPersistanceRegistration();
builder.Services.ConfigureEventHandlers();
builder.Services.AddEventBusService();
builder.Services.AddLogging(configure => configure.AddConsole());
builder.Services.ConfigureConsul();
builder.Services.ConfigureAuth();

 

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(connectionString));
 


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.MigrateDbContext<OrderDbContext>((context, services) =>
{
    var env = services.GetService<IWebHostEnvironment>();
    var logger = services.GetService<ILogger<OrderDbContextSeed>>();

    new OrderDbContextSeed()
        .SeedAsync(context, logger)
        .Wait();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();




app.Start();
app.RegisterWithConsul(builder.Services);
app.WaitForShutdown();