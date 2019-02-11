using System;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Url.AbsoluteUri.ToLower() == "http://cuplex.se/Default.aspx".ToLower())
        {
            Response.Redirect("http://" + cms.Current.DomainUrl);
            return;
        }
        else if(Request.Url.AbsoluteUri.ToLower().Contains("https://www.cuplex.se"))
        {
            Response.Redirect("https://" + cms.Current.HTTPS_DomainUrl);
        }

        //EventLog.SaveToEventLog(Request.Url.AbsoluteUri, EventLogType.Information, null);
        Server.TransferRequest("StartPage.aspx");
    }   
}