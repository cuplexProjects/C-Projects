using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CuplexProject.Startup))]
namespace CuplexProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
