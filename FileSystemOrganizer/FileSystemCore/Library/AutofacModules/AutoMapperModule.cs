using System.Collections.Generic;
using Autofac;
using AutoMapper;
using Hangfire.Annotations;

namespace FileSystemCore.Library.AutofacModules
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    [UsedImplicitly]
    public class AutoMapperModule : Module
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<GeneralToolkitProfile>().As<Profile>();
            builder.Register(Configure)
                   .AutoActivate()
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.Register(context => context.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>().AutoActivate().SingleInstance();
        }

        /// <summary>
        /// Configures the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private static MapperConfiguration Configure(IComponentContext context)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                var innerContext = context.Resolve<IComponentContext>();
                cfg.ConstructServicesUsing(innerContext.Resolve);

                foreach (var profile in context.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            });

            return configuration;
        }
    }
}