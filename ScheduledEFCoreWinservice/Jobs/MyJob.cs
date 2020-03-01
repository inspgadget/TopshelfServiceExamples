using Quartz;
using ScheduledEFCoreWinservice.Controllers;
using ScheduledEFCoreWinservice.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledEFCoreWinservice.Jobs
{
    public class MyJob : IJob
    {
        private readonly JobController _jc;
        private readonly ip2locationContext _ctx;

        public MyJob(JobController jc, ip2locationContext ctx)
        {
            _jc = jc;
            _ctx = ctx;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine();
            Console.WriteLine($"*** {context.JobDetail.Key} ***");
            _jc.ProcessData(context.JobDetail.JobDataMap);
            Ip2locationDb11 obj = _ctx.Ip2locationDb11.Skip(1).Take(1).FirstOrDefault();
            Console.WriteLine($"DB Query CONTEXT: " + obj.CityName);
            return Task.CompletedTask;
        }
    }
}
