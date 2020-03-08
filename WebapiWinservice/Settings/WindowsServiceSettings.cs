using Microsoft.Extensions.Configuration;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace WebapiWinservice.Settings
{
    public class WindowsServiceSettings : SettingsBase
    {
        public WindowsServiceSettings(IConfiguration configuration)
            :base(configuration)
        { }

        public string RunAs { get; set; } 
        public string RunAsPassword { get; set; }  
    }
}
