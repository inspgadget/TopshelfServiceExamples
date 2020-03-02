## Required NuGet-Packages
* [Topshelf](http://topshelf-project.com/)
* Topshel.Autofac
* Microsoft.Extensions.Configuration.Json
* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.SqlServer
* Microsoft.EntityFrameworkCore.Tools
* Microsoft.AspNetCore
* Microsoft.AspNetCore.Core
* Microsoft.AspNetCore.Server.Kestrel.Core
* Microsoft.Extensions.Hosting
* Microsoft.Extensions.Logging.Configuration

## Topshelf
* Executing the exe will run the service in a console
* Windowsservice can be installed via cmd.exe (Run as Administrator) - `Service.exe install`
* For uninstallation use `Service.exe uninstall`

## Project-setup
1.  Console-App (.NET Core 3.1)
* Is used for the windowsservice
* Open Projectfile - Edit project Sdk to: `<Project Sdk="Microsoft.NET.Sdk.Web">`
2.  Classlibrary (.NET STandard 2.0)
* If responceobjects are desired for a C# Application create a class library. Alternativly you can just use dynmic objects parsed from JSON.

## appsettings.json
* The section `Kestrel` and `Logging` must be configured
* "AllowedHosts": "192.168.130.16;test.example.com" - with that configuration you can define from which hosts the WebApi can be accessed

## Publish hints
* AllowedHosts should be configured
* Wenn Api nur intern verfügbar sein soll, Windows-Firewall konfigurieren

## Debugging
**!!! Do not execute with IIS-Express !!!**

![debugging](https://raw.githubusercontent.com/inspgadget/TopshelfServiceExamples/master/Wiki/debug.jpg)

## Bootstrap.cs
This class is for registrering the types for the dependency Injection.

### BuildContainer
* Registration of the servicecontroller

### RegisterGlobalTypes
These types must be registered 2 times. Once for the Asp.Net webhost and once for your the service controller.

* Registration of IConfiguration - Provides reading the `appsettings.json`
* Registration of Settings - Helperclass for easier access of the configuration

### WebApi
#### Initialisation
**Building the webhost**

Define the settings for the kestrel server:
```csharp
_webHost = Host.CreateDefaultBuilder()
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
        webBuilder.ConfigureKestrel(serverOptions =>
        {
            serverOptions.Limits.MaxConcurrentConnections = 100;
            serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
            serverOptions.Limits.MaxRequestBodySize = 30 * 1024 * 1024;
            serverOptions.Limits.MinRequestBodyDataRate =
                new MinDataRate(bytesPerSecond: 100,
                    gracePeriod: TimeSpan.FromSeconds(10));
            serverOptions.Limits.MinResponseDataRate =
                new MinDataRate(bytesPerSecond: 100,
                    gracePeriod: TimeSpan.FromSeconds(10));
            serverOptions.Limits.KeepAliveTimeout =
                TimeSpan.FromMinutes(2);
            serverOptions.Limits.RequestHeadersTimeout =
                TimeSpan.FromMinutes(1);
        });
    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .Build();
```

**Startup.cs**

*ConfigureContainer*

Registration of the types for Autofac
```csharp
public void ConfigureContainer(ContainerBuilder builder)
{
    Bootstrap.RegisterGlobalTypes(builder);
    builder.RegisterType<Ip2LocationController>();
    // is used for determining the client ip
    builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
    // context per request
    builder.RegisterType<ip2locationContext>().InstancePerLifetimeScope();
}
```

*Configure*

If HostHttpsRedirect is set to «1», the service will redirect to HTTPS automatically.
```csharp
Settings settings = app.ApplicationServices.GetAutofacRoot().Resolve<Settings>();
if (settings.GetAppSetting("HostHttpsRedirect") == "1")
{
    app.UseHttpsRedirection();
}
```

#### Start / Stop
```csharp
// Start
_webHost.StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();
// Stop 
_webHost.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
```

#### ApiController
1.  Inherits from ControllerBase
2.  Set the attributes `Route` & `ApiController`
3.  Implement `HttpGet` / `HttpPost` methods

#### Exampleclient
Project «WebapiWinserviceClientExample»

## EF Core with DependencyInjection

### Bootstrap
`InstancePerLifetimeScope` - the context will be created for each job and after the job is done it will be disposed
```csharp
builder.RegisterType<ip2locationContext>().InstancePerLifetimeScope();
builder.RegisterType<ip2locationWriteContext>().InstancePerLifetimeScope();
```

### Context
The configuration will be automatically injected because fo the constructor `ip2locationContext(IConfiguration configuration)`. The configuration is used for reading the connection string.
```csharp
public partial class ip2locationContext : DbContext
{
    protected readonly IConfiguration _configuration;

    public ip2locationContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("CTX"));
        }
    }
}
```

### ip2locationContextFactory
If you want EF-Core migrations, you should implement this class. Othweriws the EF-Core tools won´t find you connection string.
