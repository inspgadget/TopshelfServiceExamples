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
        
        // This method gets called by the runtime. Use this method to add services to the container.
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

        // This method gets called by the runtime because AutoFacProvider
        public void ConfigureContainer(ContainerBuilder builder) => builder.Config().Setup();

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var hostingSettings = app.ApplicationServices.GetAutofacRoot().Resolve<HostingSettings>();
            if (env.IsDevelopment() || hostingSettings.UseDeveloperExceptionPage)
                app.UseDeveloperExceptionPage();
            //else
                //app.UseExceptionHandler("/Error");

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
