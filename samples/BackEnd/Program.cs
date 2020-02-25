using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Hosting;

namespace BackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseOrleans((ctx, siloBuilder) =>
                {
                    var cfg = ctx.Configuration;

                    var gwListenPort = int.Parse(cfg["GATEWAY_INTERNAL_PORT"]);
                    var siloListenPort = int.Parse(cfg["SILO_INTERNAL_PORT"]);
                    siloBuilder.ConfigureEndpoints(siloListenPort, gwListenPort, listenOnAnyHostAddress: true);
                    siloBuilder.Configure<ClusterOptions>(o => o.ClusterId = o.ServiceId = "dev");

                    var redisConnectionString = $"{cfg["SERVICE:REDIS:HOST"]}:{cfg["SERVICE:REDIS:PORT"]}";
                    siloBuilder.UseRedisMembership(redisConnectionString);

                    siloBuilder.AddMemoryGrainStorageAsDefault();
                });
    }
}
