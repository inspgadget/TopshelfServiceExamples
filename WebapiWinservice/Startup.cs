using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebapiWinservice.Settings;

namespace WebapiWinservice
{
    public class Startup
    {
        public void ConfigureContainer(ContainerBuilder builder) => Bootstrapper.Setup(builder);
           
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
            var hostingSettings = app.ApplicationServices.GetAutofacRoot().Resolve<HostingSettings>();
            if (env.IsDevelopment() || hostingSettings.UseDeveloperExceptionPage)
                app.UseDeveloperExceptionPage();

            if (hostingSettings.HttpsRedirection)
                app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
