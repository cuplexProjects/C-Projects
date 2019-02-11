using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using CuplexLib;
using EventLog = CuplexLib.EventLog;

namespace cms
{
    public class HttpModule : IHttpModule
    {
        public void Dispose() { }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(this.context_BeginRequest);
            context.EndRequest += new EventHandler(this.context_EndRequest);           
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;

            try
            {
                string path = app.Request.Url.LocalPath.Substring(app.Request.ApplicationPath.Length);
                path = this.ensureBeginsWithSlash(path);
                Url requestUrl = new Url(path, app.Request.QueryString);


                //Debug Code -------------------
                //app.Response.Write("Nu körs en request, sökväg:" + app.Request.Url.AbsolutePath);
                //CuplexLib.EventLog.SaveToEventLog(app.Request.Url.AbsoluteUri, CuplexLib.EventLogType.Information, null);
                //CuplexLib.EventLog.SaveToEventLog("sökväg->" + path, EventLogType.Information, null);
                //Debug Code -------------------

                
                if (requestUrl.MatchPath("/go/<int>"))
                {
                    int linkRef;
                    int length = Math.Min(64, path.Length);
                    string strLinkRef = path.Substring(4, length - 4);
                    if (int.TryParse(strLinkRef, out linkRef))
                    {
                        try
                        {
                            string redirectUrl = cms.Current.GetRootPath;
                            using (CuplexLib.Linq.DataContext db = CuplexLib.Linq.DataContext.Create())
                            {
                                var link = db.Links.SingleOrDefault(l => l.LinkRef == linkRef);
                                if (link != null)
                                {
                                    redirectUrl = link.LinkUrl;
                                    link.Clicks++;
                                    db.SubmitChanges();
                                }
                            }
                            app.Response.Redirect(redirectUrl);
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = ex.Message;                           
                        }
                    }
                }
                else if (requestUrl.MatchPath("/page/<int>"))
                {
                    int pageId;
                    int length = Math.Min(64, path.Length);
                    string strPageId = path.Substring(6, length - 6);
                    if (int.TryParse(strPageId, out pageId))
                    {
                        try
                        {
                            string pathToUse = this.removeEndingSlash(app.Request.ApplicationPath) + "/StartPage.aspx?pageId=" + pageId;
                            app.Context.RewritePath(pathToUse);
                        }
                        catch (Exception ex) 
                        {
                            string errorMessage = ex.Message;
                            EventLog.SaveToEventLog("'/page/<int>' " + errorMessage, EventLogType.Error, null);
                        }
                    }
                }
                else if (requestUrl.MatchPath("/page/<*>"))
                {
                    try
                    {
                        string pathToUse = this.removeEndingSlash(app.Request.ApplicationPath) + requestUrl.Path.Replace("/page", "");
                        app.Context.RewritePath(pathToUse);
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = ex.Message;
                        EventLog.SaveToEventLog("/page/<*>' " + errorMessage, EventLogType.Error, null);
                    }
                }
                else if (requestUrl.MatchPath("/user/login"))
                {
                    app.Response.Redirect(Current.GetRootPath + "ManageUser.aspx?action=login");
                }
                else if (requestUrl.MatchPath("/user/createuser"))
                {
                    app.Response.Redirect(Current.GetRootPath + "ManageUser.aspx?action=createuser");
                }
                else if (requestUrl.MatchPath("/logout"))
                {
                    app.Response.Redirect(Current.GetRootPath + "StartPage.aspx?action=logout");
                }
                else if (requestUrl.MatchPath("/link"))
                {
                    app.Response.Redirect(Current.GetRootPath + "SuggestLink.aspx");
                }
                else if (requestUrl.MatchPath("/user/settings"))
                {
                    app.Response.Redirect(Current.GetRootPath + "AccountSettings.aspx");
                    //app.Context.RewritePath(app.Request.ApplicationPath + "/AccountSettings.aspx");
                }
                else if (requestUrl.MatchPath("/admin"))
                {
                    //app.Context.RewritePath(removeEndingSlash(app.Request.ApplicationPath) + "/AdminPage.aspx");
                    app.Response.Redirect(Current.GetRootPath + "AdminPage.aspx");
                }
                else if (requestUrl.MatchPath("/settings"))
                {                   
                    //app.Response.Redirect(Current.GetRootPath + "Settings.aspx");
                    string pathToUse;
                    if (path == "/settings/")
                        pathToUse = this.removeEndingSlash(app.Request.ApplicationPath) + "/Settings.aspx";
                    else
                        pathToUse = this.removeEndingSlash(app.Request.ApplicationPath) + requestUrl.Path.Replace("/settings", "");

                    app.Context.RewritePath(pathToUse);                   
                }
                else if (requestUrl.MatchPath("/search"))
                {
                    string pathToUse;
                    if (path == "/search/")
                        pathToUse = this.removeEndingSlash(app.Request.ApplicationPath) + "/StartPage.aspx";
                    else
                        pathToUse = this.removeEndingSlash(app.Request.ApplicationPath) + requestUrl.Path.Replace("/search", "");
                    
                    app.Context.RewritePath(pathToUse);                    
                }
                else if (requestUrl.MatchPath("/randimg"))
                {
                    string pathToUse;
                    if (path == "/randimg/")
                        pathToUse = this.removeEndingSlash(app.Request.ApplicationPath) + "/RandomImage.aspx";
                    else
                    {
                        if (requestUrl.Path.Substring(9).Contains('/'))
                        {
                            pathToUse = this.removeEndingSlash(app.Request.ApplicationPath) + "/" + requestUrl.Path.Substring(9);
                        }
                        else if (requestUrl.Path == "/randimg/RandomImage.aspx")
                        {
                            pathToUse = this.removeEndingSlash(app.Request.ApplicationPath) + "/RandomImage.aspx";
                        }
                        else
                        {
                            pathToUse = this.removeEndingSlash(app.Request.ApplicationPath) + "/RandomImage.aspx?img=" + requestUrl.Path.Substring(9);
                        }
                    }
                    //pathToUse = removeEndingSlash(app.Request.ApplicationPath) +"/RandomImage.aspx?img=" + requestUrl.Path.Replace("/randimg", "");

                    app.Context.RewritePath(pathToUse);
                }
                else if (requestUrl.MatchPath("/about"))
                {
                    string pathToUse;
                    if (path == "/about/")
                        pathToUse = this.removeEndingSlash(app.Request.ApplicationPath) + "/AboutPage.aspx";
                    else
                        pathToUse = this.removeEndingSlash(app.Request.ApplicationPath) + requestUrl.Path.Replace("/about", "");

                    app.Context.RewritePath(pathToUse);
                }
                //else if (requestUrl.MatchPath("/utils"))
                //{
                //    string pathToUse;
                //    if (path == "/utils/")
                //        pathToUse = removeEndingSlash(app.Request.ApplicationPath) + "/Utils.aspx";
                //    else
                //        pathToUse = removeEndingSlash(app.Request.ApplicationPath) + requestUrl.Path.Replace("/utils", "");

                //    app.Context.RewritePath(pathToUse);
                //}
            }
            catch (Exception ex) { app.Response.Write(ex.Message); }
        }
        void context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
        }

        private string ensureBeginsWithSlash(string path)
        {
            if (path.Length > 0)
            {
                if (path[0] != '/')
                    return '/' + path;
            }
            return path;
        }
        private string removeEndingSlash(string url)
        {
            if (url.EndsWith("/"))
                return url.Substring(1);
            else
                return url;
        }
    }

    public class Url
    {
        private string _pageName = "";
        private string _path = "";
        public QueryString QueryString { get; set; }
        public string PageName
        {
            get { return this._pageName; }
            private set { this._pageName = value; }
        }
        public string Path
        {
            get { return this._path; }
        }
        public Url(string path, System.Collections.Specialized.NameValueCollection query)
        {
            this.QueryString = new QueryString(query);
            this._path = path;

            if (path.StartsWith("/")) path = path.Substring(1);
        }

        public bool MatchPath(string urlQuery)  //"/go/<int>"
        {
            string pattern = urlQuery.Replace("<int>", @"\d{1,10}\b");
            if (pattern.EndsWith("<*>"))
                pattern = pattern.Replace("<*>", "");
            else if (!pattern.EndsWith(@"\b"))
                pattern += @"\b";
            Regex regEx = new Regex(pattern);
            return regEx.IsMatch(this._path);
        }

        private class UrlSegment
        {
        }

    }
    public class QueryString : System.Collections.Specialized.NameValueCollection
    {
        public QueryString(System.Collections.Specialized.NameValueCollection queryString)
        {
            this.CopyFrom(queryString);
        }

        public void CopyFrom(System.Collections.Specialized.NameValueCollection collection)
        {
            foreach (string key in collection.AllKeys)
            {
                this[key] = collection[key];
            }
        }

        public void Insert(string name, string value)
        {
            this[name] = value;
        }
        public override string ToString()
        {
            string str = this.AllKeys.Aggregate("?", (current, key) => current + (key + "=" + this[key] + "&"));
            if (str == "?") return string.Empty;

            return str.TrimEnd('&');
        }
    }
}
