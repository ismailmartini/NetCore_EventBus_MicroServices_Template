//var builder = WebApplication.CreateBuilder(args);



using CatalogService.Api.Extensions;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
 

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "Pics",
    ContentRootPath = Directory.GetCurrentDirectory()
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CatalogContext>(options => options.UseSqlServer(connectionString));
 
//builder.Services.AddDbContext<CatalogContext>(x => x.UseSqlServer(connectionString));


//builder.Services.AddDbContext<CatalogContext>(options => options.UseSqlServer(Microsoft.Extensions.Configuration.ConnectionString));




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MigrateDbContext<CatalogContext>((context, services) =>
{
    var env = services.GetService<IWebHostEnvironment>();
    var logger = services.GetService<ILogger<CatalogContextSeed>>();

    new CatalogContextSeed()
        .SeedAsync(context, env, logger)
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

app.Run();
