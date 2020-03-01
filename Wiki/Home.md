# Windows services with Topshelf

## Basics

### Dependency Injection
[Wikipedia](https://en.wikipedia.org/wiki/Dependency_injection)

[Dependency Injection explained](http://tutorials.jenkov.com/dependency-injection/index.html)

[Stackexchange post](https://softwareengineering.stackexchange.com/questions/19203/what-are-the-benefits-of-using-dependency-injection-and-ioc-containers)



For all examples Autofac is used.

[Autofac website](https://autofac.org/)

[Autofac documentation](https://autofaccn.readthedocs.io/en/latest/getting-started/)



## Examples
## [BasicWinservice](https://github.com/inspgadget/TopshelfServiceExamples/blob/master/Wiki/Basicwinservice.md)
Basic Windowservice

## [ScheduledWinservice](https://github.com/inspgadget/TopshelfServiceExamples/blob/master/Wiki/Scheduledwinservice.md)
Uses a [Quartz-Scheduler](https://www.quartz-scheduler.net/) for executings scheduled jobs

## [ScheduledEFCoreWinservice](https://github.com/inspgadget/TopshelfServiceExamples/blob/master/Wiki/Scheduledefcorewinservice.md)
*  [QartzScheduler](https://www.quartz-scheduler.net/)
*  Entity Framework Core (Context injection with Autofac)

## [WebapiWinservice](https://github.com/inspgadget/TopshelfServiceExamples/blob/master/Wiki/Webapiwinservice.md)
Runs a kestrel server and uses Asp.Net Core MVC for providing a RestApi
* Asp.Net Core MVC
* Entity Framework Core (Context injection with Autofac)
