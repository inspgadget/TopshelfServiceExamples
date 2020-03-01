using Quartz;
using ScheduledWinservice.Controllers;
using ScheduledWinservice.Jobs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledWinservice.Services
{


    public class ScheduledService
    {
        private readonly QuartzController _quartzController;

        public ScheduledService(QuartzController quartzController)
        {
            _quartzController = quartzController;
        }

        public async Task<bool> Start()
        {
            JobDataMap jdm = new JobDataMap();
            jdm.Add("name", "einmal ausführen");
            string jobKey = await _quartzController.ScheduleJobStartOnce<MyJob>(DateTime.Now.AddSeconds(5), jdm);

            jdm = new JobDataMap();
            jdm.Add("name", "einmal ausführen");
            jobKey = await _quartzController.ScheduleJobStartOnce<MyJob>(DateTime.Now.AddMinutes(2), jdm);
            await _quartzController.RemoveJob(jobKey);

            jdm = new JobDataMap
            {
                { "name", "Alle 5sek" }
            };
            jobKey = await _quartzController.ScheduleJobWithIntervall<MyJob>(null, new TimeSpan(0, 0, 5), jdm);

            await _quartzController.Start();

            return true;
        }

        public async Task<bool> Stop()
        {
            await _quartzController.Shutdown();
            return true;
        }
    }
}
