using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WebapiWinservice.Controllers;
using WebapiWinservice.DB;
using Microsoft.AspNetCore.Http;

namespace WebapiWinservice
{
    public class Startup
    {
        public void ConfigureContainer(ContainerBuilder builder)
        {
            Bootstrap.RegisterGlobalTypes(builder);
            builder.RegisterType<Ip2LocationController>();
            // wird für das Abrufen der ClientIp benötigt - als Singleton registrieren
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            // Context pro Request erzeugen (InstancePerLifetimeScope)
            builder.RegisterType<ip2locationContext>().InstancePerLifetimeScope();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowCredentials();
            }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Settings settings = app.ApplicationServices.GetAutofacRoot().Resolve<Settings>();
            if (settings.GetAppSetting("HostHttpsRedirect") == "1")
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
