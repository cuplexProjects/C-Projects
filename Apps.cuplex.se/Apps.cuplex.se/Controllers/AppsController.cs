using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apps.cuplex.se.Models;
using Apps.cuplex.se.Utilities;

namespace Apps.cuplex.se.Controllers
{
    public class AppsController : Controller
    {
        private readonly ApplicationListHelper _applicationListHelper;

        public AppsController(ApplicationListHelper applicationListHelper)
        {
            _applicationListHelper = applicationListHelper;
        }

        // GET: Apps
        public ActionResult Index()
        {
            var model = new AppsListViewModel
            {
                ListDescription = "Available applications",
                ApplicationsList = _applicationListHelper.GenerateList()
            };

            return View(model);
        }
    }
}