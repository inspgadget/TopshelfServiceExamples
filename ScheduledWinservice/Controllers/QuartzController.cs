using Quartz;
using Quartz.Impl.Matchers;
using ScheduledWinservice.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledWinservice.Controllers
{
    public class QuartzController
    {
        private readonly IScheduler _scheduler;

        public QuartzController(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public async Task RemoveJob(string key, string group = "DEFAULT")
        {
            IReadOnlyCollection<JobKey> keys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            JobKey jobKey = keys.FirstOrDefault(x => x.Name == key);
            if (jobKey != null)
            {
                await _scheduler.DeleteJob(jobKey);
                return;
            }
        }

        public async Task Start()
        {
            await _scheduler.Start();
        }

        public async Task Shutdown()
        {
            await _scheduler.Shutdown();
        }

        public async Task<string> ScheduleJobStartOnce<T>(DateTime start, JobDataMap jobData = null, string groupName = null) where T : IJob
        {
            long ticks = DateTime.Now.Ticks;
            var jobName = $"{typeof(T).Name}-{ticks}";
            var builder = JobBuilder
                .Create<T>()
                .WithIdentity(jobName, groupName);

            if (jobData != null)
            {
                builder = builder.SetJobData(jobData);
            }

            var job = builder.Build();


            var cronTriggerBuilder = TriggerBuilder
                .Create()
                .WithIdentity($"{jobName}-Trigger", groupName)
                .StartAt(start.ToUniversalTime())
                .WithSimpleSchedule(x => x.WithRepeatCount(0).WithMisfireHandlingInstructionFireNow())
                .ForJob(job);

            var cronTrigger = cronTriggerBuilder.Build();

            await _scheduler.ScheduleJob(job, cronTrigger);
            return jobName;
        }

        public async Task<string> ScheduleJobWithIntervall<T>(DateTime? start, TimeSpan interval, JobDataMap jobData = null, string groupName = null) where T : IJob
        {
            long ticks = DateTime.Now.Ticks;
            var jobName = $"{typeof(T).Name}-{ticks}";
            var builder = JobBuilder
                .Create<T>()
                .WithIdentity(jobName, groupName);

            if (jobData != null)
            {
                builder = builder.SetJobData(jobData);
            }

            var job = builder.Build();


            var cronTriggerBuilder = TriggerBuilder
                .Create()
                .WithIdentity($"{jobName}-Trigger", groupName);

            if (start.HasValue)
            {
                cronTriggerBuilder = cronTriggerBuilder.StartAt(start.Value.ToUniversalTime());
            }
            else
            {
                cronTriggerBuilder = cronTriggerBuilder.StartNow();
            }

            cronTriggerBuilder = cronTriggerBuilder.WithSimpleSchedule(x => x.WithInterval(interval).WithMisfireHandlingInstructionFireNow().RepeatForever())
                .ForJob(job);

            var cronTrigger = cronTriggerBuilder.Build();

            await _scheduler.ScheduleJob(job, cronTrigger);
            return jobName;
        }
    }
}
