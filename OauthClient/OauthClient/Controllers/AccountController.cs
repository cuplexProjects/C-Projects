using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using Newtonsoft.Json;
using OauthClient.Models;

namespace OauthClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly WebServerClient _client;

        private IAuthorizationState Authorization
        {
            get => (AuthorizationState)Session["Authorization"];
            set => Session["Authorization"] = value;
        }

        public AccountController()
        {
            var authServerDescription = new AuthorizationServerDescription
            {
                TokenEndpoint = new Uri(ConfigurationManager.AppSettings["Oauth:TokenEndpoint"]),
                AuthorizationEndpoint = new Uri(ConfigurationManager.AppSettings["Oauth:AuthenticationEndpoint"])
            };
            _client = new WebServerClient(authServerDescription, ConfigurationManager.AppSettings["Oauth:ClientId"], ConfigurationManager.AppSettings["Oauth:ClientSecret"]);
        }

        // GET: Account
        public ActionResult Index()
        {
            var model = new AccountViewModel();

            // If postback begin OAuth2 login 
            if (Request.RequestType == "POST")
            {
                _client.RequestUserAuthorization(new string[0]);
            }
            else
            {
                var authorization = _client.ProcessUserAuthorization();
                if (authorization != null)
                {
                    // We are receiving an authorization response.  Store it and associate it with this user.
                    Authorization = authorization;
                    Response.Redirect(Request.Path); // get rid of the /?code= parameter
                }
            }

            if (Authorization != null)
            {
                model.Mesage= "Authorization received! ";
                model.AccessToken = Authorization.AccessToken;
                model.RefreshToken = Authorization.RefreshToken;

                if (Authorization.AccessTokenExpirationUtc.HasValue)
                {
                    TimeSpan timeLeft = Authorization.AccessTokenExpirationUtc.Value - DateTime.UtcNow;
                    model.Mesage += $"AccessToken expires in {timeLeft}";
                }
            }
           
            return View(model);
        }

        public ActionResult Logout()
        {
            string logoutUrl = ConfigurationManager.AppSettings["Oauth:LogoutUri"];
            var builder = new UriBuilder(logoutUrl);
            var helper = new UrlHelper(Request.RequestContext);
            builder.AppendQueryArgument("clientId", ConfigurationManager.AppSettings["Oauth:ClientId"]);
            builder.AppendQueryArgument("returnUrl", helper.Action("Index", "Account", null, "http"));
            builder.AppendQueryArgument("errorUrl", helper.Action("Index", "Account", null, "http"));
            Authorization = null;

            return Redirect(builder.ToString());
        }

        public async Task<ActionResult> MediaconnectProfile()
        {
            var model = new MediaconectProfileViewModel();
            if (Authorization != null)
            {
                model.IsAuthenticated = true;
                MediaconnectProfile profile = await GetMediaconnectProfileData(Authorization.AccessToken);
                model.FirstName = profile.name.firstName;
                model.MiddleName = profile.name.middleName;
                model.LastName = profile.name.lastName;
                model.CustomerNumber = profile.customerNumber.ToString();
                model.EmailAddres = profile.emails.FirstOrDefault();
                model.UniqueId = profile.uniqueId;
            }


            return View(model);
        }

        private async Task<MediaconnectProfile> GetMediaconnectProfileData(string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var completeUri = new Uri(ConfigurationManager.AppSettings["Oauth:ApiEndpoint"] + "/v1/customer/profile?access_token=" + token);

                string response = await client.GetStringAsync(completeUri);
                var profile = JsonConvert.DeserializeObject<MediaconnectProfile>(response);
                return profile;
            }
        }
    }
}