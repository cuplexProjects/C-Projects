using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TicketDownloadSite.Startup))]
namespace TicketDownloadSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
