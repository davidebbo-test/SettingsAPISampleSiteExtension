using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SettingsAPISample.Startup))]

namespace SettingsAPISample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
