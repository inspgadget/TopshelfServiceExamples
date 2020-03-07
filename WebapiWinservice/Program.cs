using System;
using Topshelf;
using Topshelf.Autofac;
using WebapiWinservice.Services;
using WebapiWinservice.Settings;

namespace WebapiWinservice
{
    internal static class Program
    {
        internal static void Main()
        {
            var container = Bootstrapper.Start();

            var exitCode = HostFactory.Run(x =>
            {
                x.SetServiceName("WebapiService");
                x.SetDisplayName("Webapi Service");
                x.SetDescription("Some fun stuff");
                x.UseAutofacContainer(container);

                var config = Bootstrapper.Resolve<WindowsServiceSettings>();

                if (!string.IsNullOrWhiteSpace(config.RunAs))
                    x.RunAs(config.RunAs, config.RunAsPassword);

                x.Service<WebapiService>(y =>
                {
                    y.ConstructUsingAutofacContainer();
                    y.WhenStarted(async service => await service.Start().ConfigureAwait(false));
                    y.WhenStopped(async service => await service.Stop().ConfigureAwait(false));
                });
            });

            Bootstrapper.Stop();

            //return correct exit code
            var exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
