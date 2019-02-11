using System.Web.Mvc;
using System.Web.Routing;
using JetBrains.Annotations;

namespace Cuplex.Web.Cdn
{
	[UsedImplicitly]
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			// Register the image provers routes.
			ImageProvider.RouteConfig.RegisterRoutes(routes);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
