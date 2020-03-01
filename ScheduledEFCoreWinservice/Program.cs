using Autofac;
using Quartz.Logging;
using ScheduledEFCoreWinservice.Services;
using System;
using Topshelf;
using Topshelf.Autofac;

namespace ScheduledEFCoreWinservice
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
                x.SetServiceName("ScheduledEFCoreService");
                x.SetDisplayName("ScheduledEFCoreService");
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
                        return service.Start().ConfigureAwait(false).GetAwaiter().GetResult();
                    });
                });
            });
        }
    }
}
