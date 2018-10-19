using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmartCuplexCdn.Startup))]
namespace SmartCuplexCdn
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
