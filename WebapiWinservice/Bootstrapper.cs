using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Http;
using WebapiWinservice.DB;
using WebapiWinservice.Services;
using WebapiWinservice.Settings;

namespace WebapiWinservice
{
    public static class Bootstrapper
    {
        private static IContainer _rootScope;
        public static IContainer Start()
        {
            if (_rootScope != null) return _rootScope;

            _rootScope = new ContainerBuilder()
                .Config()
                .Setup()
                .Build();

            return _rootScope;
        }
        public static void Stop()
        {
            _rootScope?.Dispose();
        }
        public static T Resolve<T>()
        {
            if (_rootScope == null)
            {
                throw new Exception("Bootstrapper hasn't been started!");
            }
            return _rootScope.Resolve<T>();
        }


        public static ContainerBuilder Config(this ContainerBuilder builder)
        {
            var path = new FileInfo(Process.GetCurrentProcess().MainModule?.FileName ?? throw new Exception()).DirectoryName;
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            builder.Register(x => 
                    new ConfigurationBuilder()
                        .SetBasePath(path)
                        //.AddEnvironmentVariables("DOTNET_")
                        .AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                        //.AddUserSecrets(Assembly.GetEntryAssembly(), true, true)
                        .AddEnvironmentVariables()
                        //.AddEnvironmentVariables("MYOWNPREFIX_")
                        .Build())
                .As<IConfiguration>();
            return builder;

            //comment config stuff are examples for further configuration.
        }



        /// <summary>
        /// Wird von Topshelf und Webapi verwendet 
        /// (Webhost erzeugt eigenen Autofac Container)
        /// </summary>
        /// <param name="builder"></param>
        public static ContainerBuilder Setup(this ContainerBuilder builder)
        {
            //settings classes
            builder.RegisterAssemblyTypes(typeof(SettingsBase).Assembly)
                .Where(t => t.IsSubclassOf(typeof(SettingsBase)))
                .AsSelf();

            //winservice
            builder.RegisterType<WebapiService>();

            //other services
            builder.RegisterType<Ip2LocationService>();
            // needed to resolve ClientIp - register as Singleton
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            // Context per Request (InstancePerLifetimeScope)
            builder.RegisterType<ip2locationContext>().InstancePerLifetimeScope();

            return builder;
        }






    }
}
