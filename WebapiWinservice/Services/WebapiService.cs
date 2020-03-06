using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace WebapiWinservice.Services
{
    public class WebapiService
    {
        private IHost _webHost;

        public async Task<bool> Start()
        {
            _webHost = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Limits.MaxConcurrentConnections = 100;
                        serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
                        serverOptions.Limits.MaxRequestBodySize = 30 * 1024 * 1024;
                        serverOptions.Limits.MinRequestBodyDataRate =
                            new MinDataRate(bytesPerSecond: 100,
                                gracePeriod: TimeSpan.FromSeconds(10));
                        serverOptions.Limits.MinResponseDataRate =
                            new MinDataRate(bytesPerSecond: 100,
                                gracePeriod: TimeSpan.FromSeconds(10));
                        serverOptions.Limits.KeepAliveTimeout =
                            TimeSpan.FromMinutes(2);
                        serverOptions.Limits.RequestHeadersTimeout =
                            TimeSpan.FromMinutes(1);
                    });
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .Build();

            await _webHost.StartAsync();
            return true;
        }

        public async Task<bool> Stop()
        {
            await _webHost.StopAsync();
            _webHost.Dispose();
            return true;
        }
    }
}
