using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestSite.Startup))]
namespace TestSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
