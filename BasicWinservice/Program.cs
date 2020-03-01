using Autofac;
using BasicWinservice.Services;
using System;
using Topshelf;
using Topshelf.Autofac;

namespace BasicWinservice
{
    class Program
    {
        static void Main(string[] args)
        {
            IContainer container = Bootstrap.BuildContainer();

            HostFactory.Run(x =>
            {
                x.SetServiceName("BasicService");
                x.SetDisplayName("BasicService");
                x.SetDescription("");
                x.UseAutofacContainer(container);
                x.Service<BasicService>(y =>
                {
                    y.ConstructUsingAutofacContainer();
                    y.WhenStarted((service, control) =>
                    {
                        return service.Start();
                    });
                    y.WhenStopped((service, control) =>
                    {
                        return service.Stop();
                    });
                });
            });
        }
    }
}
