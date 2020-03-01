using Autofac;
using Quartz.Logging;
using ScheduledWinservice.Services;
using System;
using Topshelf;
using Topshelf.Autofac;

namespace ScheduledWinservice
{
    class Program
    {
        static void Main(string[] args)
        {
            LogProvider.SetCurrentLogProvider(new QuartzConsoleLogProvider());

            IContainer container = Bootstrap.BuildContainer();
            Settings settings = container.Resolve<Settings>();

            HostFactory.Run(x =>
            {
                x.SetServiceName("ScheduledService");
                x.SetDisplayName("ScheduledService");
                x.SetDescription("");
                x.UseAutofacContainer(container);
                if (!string.IsNullOrWhiteSpace(settings.GetAppSetting("RunAsUser")))
                {
                    x.RunAs(settings.GetAppSetting("RunAsUser"), settings.GetAppSetting("RunAsPassword"));
                }

                x.Service<ScheduledService>(y =>
                {
                    y.ConstructUsingAutofacContainer();
                    y.WhenStarted((service, control) =>
                    {

                        return service.Start().ConfigureAwait(false).GetAwaiter().GetResult();
                    });
                    y.WhenStopped((service, control) =>
                    {
                        return service.Stop().ConfigureAwait(false).GetAwaiter().GetResult();
                    });
                });
            });
        }
    }
}
