using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace BasketService.Api.Extensions
{
    public static class RedisRegistiration
    {
        public static IServiceCollection ConfigureRedis(this IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            ConnectionMultiplexer conn = ReadRedisConfig(configuration);
            services.AddSingleton(conn);
            return services;

        }
        private static ConnectionMultiplexer ReadRedisConfig( IConfiguration configuration)
        {
            var redisConf = ConfigurationOptions.Parse(configuration["RedisSettings:ConnectionString"], true);
            redisConf.ResolveDns = true;
            return ConnectionMultiplexer.Connect(redisConf);
        }


       


    }
}
