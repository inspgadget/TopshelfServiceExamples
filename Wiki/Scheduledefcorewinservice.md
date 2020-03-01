## Required NuGet-Packages
* [Topshelf](http://topshelf-project.com/)
* Topshel.Autofac
* [Quartz](https://www.quartz-scheduler.net/)
* Autofac.Extras.Quartz
* Microsoft.Extensions.Configuration.Json
* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.SqlServer
* Microsoft.EntityFrameworkCore.Tools

## Topshelf
* Executing the exe will run the service in a console
* Windowsservice can be installed via cmd.exe (Run as Administrator) - `Service.exe install`
* For uninstallation use `Service.exe uninstall`

## Bootstrap.cs
This class is for registrering the types for the dependency Injection.
* Registration of Servicecontrollers
* Registration of IConfiguration - Provides reading the `appsettings.json`
* Registration of Settings - Helperclass for easier access of the configuration
* Registration of the QuartzModule
* Registration of the Controller
* Registration of the database context

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

## Quartz

### Logging
If jobs are not executed, turn on the logging of Quartz. The `QuartzConsoleLogProvider.cs` implements a logger which is logging to the console.

**Registration of the logprovider**
```csharp
LogProvider.SetCurrentLogProvider(new QuartzConsoleLogProvider());
```

### Jobs
Job are the task which your service should run. These must implement the `IJob` interface.
If this job needs some additional data you can use the `JobDataMap`. (see «QuartzController.cs» and `MyJob.cs`)

### Triggers
Quarzt provides several triggers for executing jobs at a certain time.
Examples are provided in `QuartzController.cs`:
* Execute job once
* Executing job with intervall
* Delete a job from queue

[Documentation of triggers](https://quartznet.sourceforge.io/apidoc/3.0/html/)
