using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduledWinservice
{
    public class Settings
    {
        IConfiguration _configuration;

        public Settings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetAppSetting(string key)
        {
            return _configuration.GetSection("AppSettings")?[key];
        }
    }
}
