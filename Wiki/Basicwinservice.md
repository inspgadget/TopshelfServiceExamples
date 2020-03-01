## Required NuGet-Packages
* [Topshelf](http://topshelf-project.com/)
* Topshel.Autofac

## Topshelf
* Executing the exe will run the service in a console
* Windowsservice can be installed via cmd.exe (Run as Administrator) - `Service.exe install`
* For uninstallation use `Service.exe uninstall`

## Bootstrap.cs
This class is for registrering the types for the dependency Injection.
In this example the `BasicService`, which contains the logic for the service,  must be registred.


