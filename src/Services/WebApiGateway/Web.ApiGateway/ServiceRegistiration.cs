using Web.ApiGateway.Infrastructure;

namespace Web.ApiGateway
{
    public static class ServiceRegistiration
    {
       public static void ConfigureHttpClient(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpClient("basket", c =>
            {
                c.BaseAddress = new Uri(configuration["urls:basket"]);
            })
            .AddHttpMessageHandler<HttpClientDelegatingHandler>();
            services.AddHttpClient("catalog", c =>
            {
                c.BaseAddress = new Uri(configuration["urls:catalog"]);
            })
            .AddHttpMessageHandler<HttpClientDelegatingHandler>();
        }
    }
}
