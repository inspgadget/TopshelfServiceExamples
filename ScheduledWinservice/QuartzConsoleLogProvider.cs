using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduledWinservice
{
    public class QuartzConsoleLogProvider : ILogProvider
    {
        public Logger GetLogger(string name)
        {
            return (level, func, exception, parameters) =>
            {
                if (/*level >= LogLevel.Info && */func != null)
                {
                    Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                }
                return true;
            };
        }

        public IDisposable OpenNestedContext(string message)
        {
            throw new NotImplementedException();
        }

        public IDisposable OpenMappedContext(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
