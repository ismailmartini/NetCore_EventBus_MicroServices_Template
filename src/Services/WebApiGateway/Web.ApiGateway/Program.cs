using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Values;
using Web.ApiGateway;
using Web.ApiGateway.Services;
using Web.ApiGateway.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("Configurations/ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration).AddConsul();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
//basket and catalog added uri
builder.Services.ConfigureHttpClient(builder.Configuration);

builder.Services.AddScoped<ICatalogService,CatalogService>();
builder.Services.AddScoped<IBasketService,BasketService>();


builder.Services.AddCors(options => {
    options.AddPolicy("CorsPolicy",
        builder => builder.SetIsOriginAllowed((host) => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseOcelot().Wait();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

  