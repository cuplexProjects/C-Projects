using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CuplexLib;

/// <summary>
/// Summary description for AccessControl
/// </summary>
public class AccessControl
{
    private const int DefaultAutoLoginTime = 24; //Hours
    public AccessControl()
    {

    }

    public static void CreateUserSession(string userName)
    {
        int autoLoginHours = GetAutoLoginTime();

        HttpContext.Current.Response.Cookies["UserId"].Value = userName;
        HttpContext.Current.Response.Cookies["UserId"].Expires = DateTime.Now.AddHours(autoLoginHours);

        HttpContext.Current.Response.Cookies["CuplexAuthCookie"].Value = CreateAuthenticationTicket(userName);
        HttpContext.Current.Response.Cookies["CuplexAuthCookie"].Expires = DateTime.Now.AddHours(autoLoginHours);

        User user = User.GetOneByUserName(userName);
        user.LastLogin = DateTime.Now;
        user.save();
        HttpContext.Current.Session["User"] = user;
        FormsAuthentication.SetAuthCookie(userName, true);
    }
    public static bool AuthenticateLoginTicket(string userName, string authenticationHash)
    {
        using (CuplexLib.Linq.DataContext db = CuplexLib.Linq.DataContext.Create())
        {
            return db.Authentications.Where(a => a.UserName == userName && a.AuthenticationHash == authenticationHash && a.Expires > DateTime.Now).Any();
        }
    }
    private static string CreateAuthenticationTicket(string userName)
    {
        string hashData = Utils.GetMd5Hash("NA5ps2hRjxMcEaQJwpw4QXZVGJP4MOxf" + DateTime.Now.Ticks + userName);
        int autoLoginHours = GetAutoLoginTime();

        using (CuplexLib.Linq.DataContext db = CuplexLib.Linq.DataContext.Create())
        {
            CuplexLib.Linq.Authentication auth = new CuplexLib.Linq.Authentication();
            auth.AuthenticationHash = hashData;
            auth.UserName = userName;
            auth.Expires = DateTime.Now.AddHours(autoLoginHours);
            db.Authentications.InsertOnSubmit(auth);
            db.SubmitChanges();

            return hashData;
        }
    }
    public static void LogoutUser()
    {
        string userName = HttpContext.Current.Request.Cookies["UserId"].Value;
        HttpContext.Current.Response.Cookies.Remove("UserId");
        HttpContext.Current.Response.Cookies.Remove("CuplexAuthCookie");
        HttpContext.Current.Session.Clear();

        using (CuplexLib.Linq.DataContext db = CuplexLib.Linq.DataContext.Create())
        {
            var authList = db.Authentications.Where(a => a.UserName == userName).ToList();
            db.Authentications.DeleteAllOnSubmit(authList);
            db.SubmitChanges();
        }
    }
    public static int GetAutoLoginTime()
    {
        int autoLoginHours = DefaultAutoLoginTime;
        Settings autoLogginTimeSetting = Settings.GetOneFromCache("AutoLoginTime");
        if (autoLogginTimeSetting != null)        
            int.TryParse(autoLogginTimeSetting.Value, out autoLoginHours);

        autoLoginHours = Math.Max(0, autoLoginHours);
        return autoLoginHours;
    }
}
