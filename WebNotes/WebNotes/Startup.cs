using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebNotes.Startup))]
namespace WebNotes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
