using System.Data.Entity;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using WebNotes.Library.Identity;

[assembly: OwinStartupAttribute(typeof(WebNotes.Startup))]

namespace WebNotes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            

            var builder = new ContainerBuilder();
            var thisAssembly = Assembly.GetCallingAssembly();

            //builder.RegisterModule(new SystemCountryCodeModule("--"));

            builder.RegisterType<AppDbContext>()
                   .AsSelf()
                   .As<DbContext>()
                   .InstancePerRequest();

            builder.RegisterType<AppUserStore>()
                   .As<IUserStore<AppUser, int>>()
                   .AsImplementedInterfaces()
                   .AsSelf()
                   .InstancePerRequest();

            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();

            builder.RegisterType<AppUserManager>()
                   .AsSelf()
                   .InstancePerRequest();

            builder.RegisterType<AppSignInManager>().AsSelf().InstancePerRequest();

            builder.Register(context => HttpContext.Current.GetOwinContext().Authentication)
                   .As<IAuthenticationManager>()
                   .InstancePerRequest();

            var config = GlobalConfiguration.Configuration;

            builder.RegisterControllers(thisAssembly);
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();
            builder.RegisterModule<AutofacWebTypesModule>();

            builder.RegisterFilterProvider();
            //builder.RegisterWebApiFilterProvider(config);

            //builder.RegisterAssemblyModules(BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray());
            
            builder.RegisterAssemblyModules(thisAssembly);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            ConfigureAuth(app);

        }
    }
}
