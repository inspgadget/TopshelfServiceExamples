using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduledWinservice.Controllers
{
    public class JobController
    {
        private readonly Settings _settings; 

        public JobController(Settings settings)
        {
            _settings = settings;
        }

        public void ProcessData(JobDataMap jdm)
        {
            foreach (string key in jdm.Keys)
            {
                Console.WriteLine($"{key} - {jdm[key]}");
            }
            Console.WriteLine($"Setting IMAGE_PATH: {_settings.GetAppSetting("IMAGE_PATH")}");
        }
    }
}
