using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.IO;

namespace WebMail.AdminPanel.PlugIns.WebMailLite
{
    public partial class show_log : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string mode = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
            {
                mode = Request.QueryString["mode"];
            }

            if (!Page.IsPostBack)
            {
                switch (mode)
                {
                    case "part":
                        ShowLogPartial();
                        break;
                    default:
                        ShowLog();
                        break;
                }
            }
        }

        protected void ShowLog()
        {
            Response.Clear();
            Response.AddHeader("Content-Type", "text/html");
            Response.Write(@"<html><body><pre class=""wm_print_content"">");
            Utils.SettingsPath = AdminPanelUtils.GetWebMailFolder();
            string logPath = Utils.GetLogFilePath();
            Response.Write(HttpUtility.HtmlEncode(Utils.LoadFromFile(logPath, Encoding.Default)));
            Response.Write("</pre></body></html>");
            Response.Flush();
            Response.Close();
        }

        protected void ShowLogPartial()
        {
            Response.Clear();
            Response.AddHeader("Content-Type", "text/html");
            Response.Write(@"<html><body><pre class=""wm_print_content"">");
            Utils.SettingsPath = AdminPanelUtils.GetWebMailFolder();
            string logPath = Utils.GetLogFilePath();
            FileInfo fi = new FileInfo(logPath);
            string log = string.Empty;
            const int lastPartSize = 20000;
            if (fi.Exists)
            {
                log = fi.Length > lastPartSize ? (string)Utils.LoadFromFile(logPath, Encoding.Default, (int)fi.Length - lastPartSize, lastPartSize) : Utils.LoadFromFile(logPath, Encoding.Default, 0, lastPartSize);
            }

            Response.Write(log);
            Response.Write("</pre></body></html>");
            Response.Flush();
            Response.Close();
        }
    }
}
