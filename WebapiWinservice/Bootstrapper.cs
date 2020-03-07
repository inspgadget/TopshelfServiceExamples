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

            var builder = new ContainerBuilder();
            Setup(builder);
            _rootScope = builder.Build();
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




        /// <summary>
        /// Wird von Topshelf und Webapi verwendet 
        /// (Webhost erzeugt eigenen Autofac Container)
        /// </summary>
        /// <param name="builder"></param>
        public static void Setup(ContainerBuilder builder)
        {
            RegisterAllSettingsStuff(builder);
            RegisterServices(builder);
        }



        private static void RegisterAllSettingsStuff(ContainerBuilder builder)
        {
            builder.Register(x => new ConfigurationBuilder()
                .SetBasePath(new FileInfo(Process.GetCurrentProcess().MainModule?.FileName ?? Directory.GetCurrentDirectory()).DirectoryName)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build()).As<IConfiguration>();

            builder.RegisterAssemblyTypes(typeof(SettingsBase).Assembly)
                .Where(t => t.IsSubclassOf(typeof(SettingsBase)))
                .AsSelf();
        }
        private static void RegisterServices(ContainerBuilder builder)
        {
            //winservice
            builder.RegisterType<WebapiService>();

            //other services
            builder.RegisterType<Ip2LocationService>();
            // wird für das Abrufen der ClientIp benötigt - als Singleton registrieren
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            // Context pro Request erzeugen (InstancePerLifetimeScope)
            builder.RegisterType<ip2locationContext>().InstancePerLifetimeScope();

        }




    }
}
