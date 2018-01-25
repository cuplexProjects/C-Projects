using System.Web.Mvc;
using System.Web.Routing;

namespace Egmont.Common.ImageProvider.Test.Mvc
{
	/// <summary>
	/// Class RouteConfig.
	/// </summary>
	public class RouteConfig
	{
		/// <summary>
		/// Registers the routes.
		/// </summary>
		/// <param name="routes">The routes.</param>
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			// Register the image provers routes.
			Provider.RouteConfig.RegisterRoutes(routes);

			routes.MapRoute(name: "Default",
							url: "{controller}/{action}/{id}",
							defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}
