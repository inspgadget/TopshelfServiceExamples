## Required NuGet-Packages
* [Topshelf](http://topshelf-project.com/)
* Topshel.Autofac
* [Quartz](https://www.quartz-scheduler.net/)
* Autofac.Extras.Quartz
* Microsoft.Extensions.Configuration.Json

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

