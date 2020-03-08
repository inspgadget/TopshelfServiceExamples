using Microsoft.Extensions.Configuration;

namespace WebapiWinservice.Settings
{
    public abstract class SettingsBase
    {
        protected SettingsBase(IConfiguration config)
        {
            config.GetSection(this.GetType().Name).Bind(this);
        }
    }
}
