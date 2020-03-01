using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using WebapiWinservice.Services;

namespace WebapiWinservice
{
    public class Bootstrap
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            RegisterGlobalTypes(builder);
            builder.RegisterType<WebapiService>();
            return builder.Build();
        }

        /// <summary>
        /// Wird von Topshelf und Webapi verwendet 
        /// (Webhost erzeugt eigenen Autofac Container)
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterGlobalTypes(ContainerBuilder builder)
        {
            builder.Register(context =>
                new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .SetBasePath(new FileInfo(Process.GetCurrentProcess().MainModule.FileName).DirectoryName)
                    .Build()
            ).As<IConfiguration>();

            builder.RegisterType<Settings>().SingleInstance();
        }
    }
}
