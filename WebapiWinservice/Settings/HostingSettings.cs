using Microsoft.Extensions.Configuration;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace WebapiWinservice.Settings
{
    public class HostingSettings : SettingsBase
    {
        public HostingSettings(IConfiguration configuration)
            :base(configuration)
        { }


        public bool UseDeveloperExceptionPage { get; set; }
        public bool HttpsRedirection { get; set; }
    }
}
