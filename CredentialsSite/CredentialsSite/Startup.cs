using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CredentialsSite.Startup))]
namespace CredentialsSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
