using System;
using Orleans;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using Orleans.Messaging;
using Orleans.Clustering.Redis;

namespace Orleans.Hosting
{
    public static class ConfigurationExtensions
    {
        public static ISiloHostBuilder UseRedisMembership(this ISiloHostBuilder builder, Action<RedisOptions> configuration)
        {
            return builder.ConfigureServices(services =>
            {
                if (configuration != null)
                {
                    services.Configure<RedisOptions>(configuration);
                }

                services.AddRedis();
            });
        }

        public static ISiloHostBuilder UseRedisMembership(this ISiloHostBuilder builder, string redisConnectionString, int db = 0)
        {
            return builder.ConfigureServices(services => services
                .Configure<RedisOptions>(options => { options.Database = db; options.ConnectionString = redisConnectionString; })
                .AddRedis());
        }

        public static ISiloBuilder UseRedisMembership(this ISiloBuilder builder, Action<RedisOptions> configuration)
        {
            return builder.ConfigureServices(services =>
            {
                if (configuration != null)
                {
                    services.Configure<RedisOptions>(configuration);
                }
                
                services.AddRedis();
            });
        }

        public static ISiloBuilder UseRedisMembership(this ISiloBuilder builder, string redisConnectionString, int db = 0)
        {
            return builder.ConfigureServices(services => services
                .Configure<RedisOptions>(options => { options.Database = db; options.ConnectionString = redisConnectionString; })
                .AddRedis());
        }

        public static IClientBuilder UseRedisMembership(this IClientBuilder builder, Action<RedisOptions> configuration)
        {
            return builder.ConfigureServices(services =>
            {
                if (configuration != null)
                {
                    services.Configure<RedisOptions>(configuration);
                }

                services
                    .AddRedis()
                    .AddSingleton<IGatewayListProvider, RedisGatewayListProvider>();
            });
        }

        public static IClientBuilder UseRedisMembership(this IClientBuilder builder, string redisConnectionString, int db = 0)
        {
            return builder.ConfigureServices(services => services
                .Configure<RedisOptions>(opt =>
                {
                    opt.ConnectionString = redisConnectionString;
                    opt.Database = db;
                })
                .AddRedis()
                .AddSingleton<IGatewayListProvider, RedisGatewayListProvider>());
        }

        private static IServiceCollection AddRedis(this IServiceCollection services)
        {
            services.AddSingleton<RedisMembershipTable>();
            services.AddSingleton<IMembershipTable>(sp => sp.GetRequiredService<RedisMembershipTable>());
            return services;
        }
    }
}
