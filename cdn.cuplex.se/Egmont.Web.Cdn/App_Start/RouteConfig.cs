using System.Web.Mvc;
using System.Web.Routing;

namespace Cuplex.Web.Cdn
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			// Register the image provers routes.
			Common.ImageProvider.RouteConfig.RegisterRoutes(routes);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
