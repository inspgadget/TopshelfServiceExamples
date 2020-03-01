using Autofac;
using System;
using Topshelf;
using Topshelf.Autofac;
using WebapiWinservice.Services;

namespace WebapiWinservice
{
    class Program
    {
        static void Main(string[] args)
        {
            IContainer container = Bootstrap.BuildContainer();
            Settings settings = container.Resolve<Settings>();

            HostFactory.Run(x =>
            {
                x.SetServiceName("WebapiService");
                x.SetDisplayName("WebapiService");
                x.SetDescription("");
                x.UseAutofacContainer(container);
                if (!string.IsNullOrWhiteSpace(settings.GetAppSetting("RunAsUser")))
                {
                    x.RunAs(settings.GetAppSetting("RunAsUser"), settings.GetAppSetting("RunAsPassword"));
                }

                x.Service<WebapiService>(y =>
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
