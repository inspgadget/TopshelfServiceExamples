using System;
using Autofac;
using Topshelf;
using Topshelf.Autofac;
using WebapiWinservice.Services;

namespace WebapiWinservice
{
    internal static class Program
    {
        internal static void Main()
        {
            var container = Bootstrap.BuildContainer();
            var settings = container.Resolve<Settings>();

            var exitCode = HostFactory.Run(x =>
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
                    y.WhenStarted(async service => await service.Start().ConfigureAwait(false));
                    y.WhenStopped(async service => await service.Stop().ConfigureAwait(false));
                });
            });

            //return correct exit code
            var exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
