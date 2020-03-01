using Quartz;
using ScheduledEFCoreWinservice.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduledEFCoreWinservice.Controllers
{
    public class JobController
    {
        private readonly Settings _settings; 
        private readonly ip2locationWriteContext _ctx;

        public JobController(Settings settings, ip2locationWriteContext ctx)
        {
            _settings = settings;
            _ctx = ctx;
        }

        public void ProcessData(JobDataMap jdm)
        {
            foreach (string key in jdm.Keys)
            {
                Console.WriteLine($"{key} - {jdm[key]}");
            }
            Console.WriteLine($"Setting IMAGE_PATH: {_settings.GetAppSetting("IMAGE_PATH")}");

            Ip2locationDb11 obj = _ctx.Ip2locationDb11.Skip(2).Take(1).FirstOrDefault();
            Console.WriteLine($"DB Query Write Context: " + obj.CityName);
        }
    }
}
