using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apps.cuplex.se.Models;
using Apps.cuplex.se.Utilities;
using Serilog;
using Serilog.Context;

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
            if (Request.Cookies["Visited"] == null)
            {
                var logdata = new
                {
                    Time = DateTime.Now,
                    Browser = Request.Browser,
                    IP = Request.UserHostAddress,
                    UserAgent = Request.UserAgent
                };

                using (LogContext.PushProperty("Data", logdata, true))
                {
                    Log.Information("AppController page render");
                }

                Response.Cookies.Add(new HttpCookie("Visited", DateTime.Now.ToString("G")));
            }

            var model = new AppsListViewModel
            {
                ListDescription = "Available applications",
                ApplicationsList = _applicationListHelper.GenerateList()
            };

            return View(model);
        }
    }
}