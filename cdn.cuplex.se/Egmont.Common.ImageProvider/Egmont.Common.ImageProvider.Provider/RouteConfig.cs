using System.Configuration;
using System.Linq;
using System.Web.Configuration;
using System.Web.Routing;
using Cuplex.Common.ImageProvider.Configuration;

namespace Cuplex.Common.ImageProvider
{
	/// <summary>
	/// Class RouteConfig.
	/// </summary>
	public static class RouteConfig
	{
		/// <summary>
		/// Registers the routes.
		/// </summary>
		/// <param name="routes">The routes.</param>
		/// <exception cref="ConfigurationErrorsException">
		/// Missing ImageProvider configuration.
		/// or
		/// Empty ImageProvider configuration.
		/// or
		/// </exception>
		public static void RegisterRoutes(RouteCollection routes)
		{
		    if (!(WebConfigurationManager.GetSection("imageProvider") is ImageProviderConfigSection section))
			{
				throw new ConfigurationErrorsException("Missing ImageProvider configuration.");
			}

			if (section.Members == null || section.Members?.Count == 0)
			{
				throw new ConfigurationErrorsException("Empty ImageProvider configuration.");
			}

			foreach (var path in section.Members.ToList())
			{
				var route = $"{path.RoutePrefix}/{{*path}}";
				routes.Add(new Route(route, new RouteHandler(path)));
			}
		}
	}
}
