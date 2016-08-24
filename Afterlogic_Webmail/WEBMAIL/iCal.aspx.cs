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
using Calendar_NET;
using System.Globalization;
using System.Text;

namespace WebMail
{
    public partial class iCal : System.Web.UI.Page
    {
        protected WebmailSettings sett = new WebmailSettings().CreateInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            iCalConstants.WEBMAIL_DOMAIN = Request.Url.Host;
            DBHelper eng = new DBHelper();

            // ---------- ICal ----------
            if (Request["ical"] != null)
            {
                string calendar_hash = Request["ical"];

                DataTable calendar = eng.SelectCalendarAccess(calendar_hash, 1);
                if (calendar != null && calendar.Rows.Count > 0 && calendar.Rows[0]["id_calendar"] != null)
                {
                    string id_calendar = calendar.Rows[0]["id_calendar"].ToString();
                    string id_user = calendar.Rows[0]["id_user"].ToString();
                    string timeZoneOffset = null;
                    string timeZoneName = null;
                    DataTable userArray = eng.Select("acal_users_data", "user_id", id_user);
                    eng._userid = int.Parse(id_user);
                    string tz = "";
                    if (userArray != null && userArray.Rows.Count > 0)
                    {
                        if (userArray.Rows[0]["timezone"] != null)
                        {
                            Constants.TimeZones tzones = new Constants.TimeZones();
                            string[] tzArr = (string[])tzones[(int) userArray.Rows[0]["timezone"]];

                            if (tzArr != null)
                            {
                                timeZoneOffset = tz = tzArr[0];
                                timeZoneName = tzArr[1];
                            }
                        }
                    }

                    DataTable calendar_data = eng.SelectCalendarForICal(id_calendar);
					int calendar_id = 0;
					string calendar_name = "";
                    string calendar_desc = "";
					string calendar_str_id = "";
					if (calendar_data != null && calendar_data.Rows.Count > 0)
                    {
						int.TryParse(calendar_data.Rows[0]["calendar_id"].ToString(), out calendar_id);
						calendar_name = Utils.ConvertFromDBString(null, calendar_data.Rows[0]["calendar_name"].ToString());
                        calendar_desc = Utils.ConvertFromDBString(null, calendar_data.Rows[0]["calendar_description"].ToString());
						calendar_str_id = Utils.ConvertFromDBString(null, calendar_data.Rows[0]["calendar_str_id"].ToString());
					}
                    DataTable eventArray = eng.SelectEventsForICal(id_calendar);

					iCalendar v = new iCalendar(calendar_id, calendar_name, calendar_desc, calendar_str_id);

                    Response.Clear();

                    string result = v.createCalendar(eventArray, timeZoneOffset);
                    
                    Response.AddHeader("Content-Type", "text/calendar; charset=UTF-8;");
					Response.AddHeader("Content-Length", Encoding.UTF8.GetByteCount(result).ToString(CultureInfo.InvariantCulture));
                    Response.AddHeader("Accept-Ranges", "bytes");
                    Response.AddHeader("Content-Disposition", "attachment; filename=\"webmail.ics\"");

                    Response.Write(result);
                    Response.End();

                    Response.Flush();
                    Response.Close();

                }
            }
            // ---------- End ICal ----------

        }
    }
}
