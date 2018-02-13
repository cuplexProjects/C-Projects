using System.Web.Routing;

namespace Egmont.Common.ImageProvider.Test.WebForms
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			// Register the image provers routes.
			Provider.RouteConfig.RegisterRoutes(routes);
		}
	}
}