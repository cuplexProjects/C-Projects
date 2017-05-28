using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ASP_Web_API.Models;
using Serilog;
using Serilog.Events;

namespace ASP_Web_API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [AllowAnonymous]
    public class AuthController : Controller
    {
        // GET: Auth
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return RedirectToAction("index", "home");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LogIn(string returnUrl)
        {
            var model = new LogInModel
            {
                ReturnUrl = returnUrl
            };

            Log.Write(LogEventLevel.Debug, "AuthController httpget -> Login request for "+ returnUrl);

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogIn(LogInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Don't do this in production!
            if (model.Email == "iddt@hotmail.com" && model.Password == "kickass")
            {
                Log.Write(LogEventLevel.Debug, "Login successful for user with email " + model.Email);
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "Martin"),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Email, "a@b.com"),
                    new Claim(ClaimTypes.Country, "Sweden")
                },
                    "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignIn(identity);

                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            // user authN failed
            Log.Write(LogEventLevel.Debug, $"Login failed for user with email {model.Email} using password {model.Password}");
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("index", "home");
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }
    }
}