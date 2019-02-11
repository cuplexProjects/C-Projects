using System;

namespace WebMail
{
    public partial class go : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
	        if (!string.IsNullOrEmpty(Request["calendar"]))
	        {
		        Response.Redirect("published-calendar.aspx?calendar=" + Request["calendar"], true);
	        }        

	        if (!string.IsNullOrEmpty(Request["ical"]))
	        {
		        Response.Redirect("ical.aspx?ical=" + Request["ical"], true);
	        }
        }
    }
}
