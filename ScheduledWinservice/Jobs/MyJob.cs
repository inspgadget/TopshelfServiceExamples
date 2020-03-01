using Quartz;
using ScheduledWinservice.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledWinservice.Jobs
{
    public class MyJob : IJob
    {
        private readonly JobController _jc;

        public MyJob(JobController jc)
        {
            _jc = jc;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine();
            Console.WriteLine($"*** {context.JobDetail.Key} ***");
            _jc.ProcessData(context.JobDetail.JobDataMap);
            return Task.CompletedTask;
        }
    }
}
