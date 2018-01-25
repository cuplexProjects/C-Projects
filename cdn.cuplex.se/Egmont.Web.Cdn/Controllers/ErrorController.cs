using System.Net;
using System.Web.Mvc;
using System.Web.WebPages;
using Cuplex.Web.Cdn.Models;

namespace Cuplex.Web.Cdn.Controllers
{
	public class ErrorController : BaseController
	{
		public ActionResult PageNotFound()
		{
			Response.SetStatus(HttpStatusCode.NotFound);
			var model = CreateModel<ErrorViewModel>("404 Not Found");
			model.Headline = model.Title;
			model.Content = "The requested resource was not found.";
			return View("ErrorMessage", model);
		}

		public ActionResult InternalServerError()
		{
			Response.SetStatus(HttpStatusCode.InternalServerError);
			var model = CreateModel<ErrorViewModel>("500 Internal Server Error");
			model.Headline = model.Title;
			model.Content = "An internal server error occurred.";
			return View("ErrorMessage", model);
		}
	}
}