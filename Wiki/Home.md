# Windows services mit Topshelf

## Grundlagen

### Dependency Injection
[Wikipedia](https://de.wikipedia.org/wiki/Dependency_Injection)

[Dependency Injection explained](http://tutorials.jenkov.com/dependency-injection/index.html)

[Stackexchange Beitrag](https://softwareengineering.stackexchange.com/questions/19203/what-are-the-benefits-of-using-dependency-injection-and-ioc-containers)



Für alle Beispiele wurde Autofac verwendet.

[Autofac Website](https://autofac.org/)

[Autofac Dokumentation](https://autofaccn.readthedocs.io/en/latest/getting-started/)


## appsettings.json
Im Debug-Modus von Visual Studio wird `appsettings.json` ansonsten wird die `appsettings.Production.json` verwenden.

## Beispiele
### [BasicWinservice](BasicWinservice)
Grundlegender Windowservice

### [ScheduledWinservice](ScheduledWinservice)
Beinhaltet einen QuartzScheduler - damit können Tasks / Jobs sehr genau ausgeführt werden

### [ScheduledEFCoreWinservice](ScheduledEFCoreWinservice)
*  QartzScheduler
*  Entity Framework Core (automatische Contextgenerierung mit Autofac DependencyInjection)

### [WebapiWinservice](WebapiWinservice)
Verwendet Asp.Net Core MVC zum Bereitstellen einer RestApi 
* Asp.Net Core MVC Webcontroller
* Entity Framework Core (automatische Contextgenerierung mit Autofac DependencyInjection)
