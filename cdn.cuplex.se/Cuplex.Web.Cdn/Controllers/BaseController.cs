using System.Web.Mvc;
using System.Web.SessionState;
using Cuplex.Web.Cdn.Models;

namespace Cuplex.Web.Cdn.Controllers
{
	[SessionState(SessionStateBehavior.Disabled)]
	public abstract class BaseController : Controller
	{
		protected TModel CreateModel<TModel>(string title) where TModel : BaseViewModel, new()
		{
			var model = new TModel { Title = title };

			return model;
		}
	}
}