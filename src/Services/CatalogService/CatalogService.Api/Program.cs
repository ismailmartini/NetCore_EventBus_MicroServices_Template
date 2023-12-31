//var builder = WebApplication.CreateBuilder(args);



using CatalogService.Api.Extensions;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using CatalogService.Api;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    //Args = args,
    //WebRootPath = "Pics2",
    //ContentRootPath = Directory.GetCurrentDirectory(),


});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CatalogContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDbContext<CatalogContext>(x => x.UseSqlServer(connectionString));


//builder.Services.AddDbContext<CatalogContext>(options => options.UseSqlServer(Microsoft.Extensions.Configuration.ConnectionString));




// Add services to the container.

builder.Services.ConfigureConsul();

builder.Services.Configure<CatalogSettings>(builder.Configuration.GetSection("CatalogSettings"));

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
 

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(builder.Environment.ContentRootPath, "Pics")),
    RequestPath = "/pics"
});
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Start();
app.RegisterWithConsul(builder.Services);
app.WaitForShutdown();