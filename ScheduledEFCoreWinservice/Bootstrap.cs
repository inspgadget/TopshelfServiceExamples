using Autofac;
using Autofac.Extras.Quartz;
using Microsoft.Extensions.Configuration;
using ScheduledEFCoreWinservice.Controllers;
using ScheduledEFCoreWinservice.DB;
using ScheduledEFCoreWinservice.Jobs;
using ScheduledEFCoreWinservice.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ScheduledEFCoreWinservice
{
    public class Bootstrap
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.Register(context =>
                new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .SetBasePath(new FileInfo(Process.GetCurrentProcess().MainModule.FileName).DirectoryName)
                    .Build()
            ).As<IConfiguration>();
            builder.RegisterType<Settings>().SingleInstance();

            var schedulerConfig = new NameValueCollection
            {
                { "quartz.scheduler.instanceName", "MyScheduler" },
                { "quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz" },
                { "quartz.threadPool.threadCount", "3" }
            };
            builder.RegisterModule(new QuartzAutofacFactoryModule
            {

                ConfigurationProvider = x => schedulerConfig

            });
            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(MyJob).Assembly));

            builder.RegisterType<ScheduledService>();
            builder.RegisterType<QuartzController>();
            builder.RegisterType<JobController>();

            builder.RegisterType<ip2locationContext>().InstancePerLifetimeScope();
            builder.RegisterType<ip2locationWriteContext>().InstancePerLifetimeScope();
            return builder.Build();
        }
    }
}
