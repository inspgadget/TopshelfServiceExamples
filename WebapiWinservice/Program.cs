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
            //get root container
            var container = Bootstrapper.Start();

            //start by Topshelf
            var exitCode = HostFactory.Run(x =>
            {
                x.UseAutofacContainer(container);
                x.Service<WebapiService>(y =>
                {
                    y.ConstructUsingAutofacContainer();
                    y.WhenStarted(async service => await service.Start().ConfigureAwait(false));
                    y.WhenStopped(async service => await service.Stop().ConfigureAwait(false));
                });
                var config = Bootstrapper.Resolve<WindowsServiceSettings>();

                //naming stuff
                x.SetServiceName("WebapiService");
                x.SetDisplayName("Webapi Service");
                x.SetDescription("Some fun stuff");
                
                //startup
                x.StartAutomaticallyDelayed();

                //user context
                if (!string.IsNullOrWhiteSpace(config.RunAs))
                    x.RunAs(config.RunAs, config.RunAsPassword);
                else
                    x.RunAsLocalSystem();
            });

            //dispose root container
            Bootstrapper.Stop();

            //return correct exit code
            var exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
