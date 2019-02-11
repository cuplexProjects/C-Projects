using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuplexLib;
using System.Net;
using System.Security.Cryptography;

public partial class UtilsPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected override void OnPreRender(EventArgs e)
    {
        if (Session["User"] == null)
        {
            LoggedInContent.Visible = false;
            NotLoggedInContent.Visible = true;
        }
        else
        {
            LoggedInContent.Visible = true;
            NotLoggedInContent.Visible = false;
        }
        base.OnPreRender(e);
    }
    protected void CheckIpButton_Clicked(object sender, EventArgs e)
    {
        IpLookup ipl = IpLookup.LookupIp(Utils.TruncateString(IpNumberTextBox.Text, 15));
        if (ipl == null) IpLookupResultLabel.Text = "Invalid ip number";
        else
            IpLookupResultLabel.Text = ipl.Country;
    }

    protected void HostLookupButton_Clicked(object sender, EventArgs e)
    {
        string hostName = Utils.TruncateString(HostNameTextBox.Text, 100);

        try
        {
            IPAddress[] ipAddress = Dns.GetHostAddresses(hostName);
            if (ipAddress.Length == 0)
                HostNameLookupResult.Text = "DNS lookup failed";
            else
                HostNameLookupResult.Text = ipAddress[0].ToString();
        }
        catch { HostNameLookupResult.Text = "DNS lookup failed"; }

    }
    protected void CreatePasswordButton_Clicked(object sender, EventArgs e)
    {
        string randomPassword = "";
        string inputStr = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        RandomNumberGenerator rndGen = null;
        if (HttpRuntime.Cache["RandomNumberGenerator"] is RandomNumberGenerator)
            rndGen = HttpRuntime.Cache["RandomNumberGenerator"] as RandomNumberGenerator;
        else
        {
            rndGen = System.Security.Cryptography.RandomNumberGenerator.Create();
            HttpRuntime.Cache.Insert("RandomNumberGenerator", rndGen, null, DateTime.Now.AddMinutes(120), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        int pwdLength = 0;
        if (int.TryParse(LengthOfPasswordTextBox.Text, out pwdLength))
        {
            pwdLength = Math.Max(Math.Min(pwdLength, 999), 0);
            byte[] buffer = new byte[pwdLength];
            rndGen.GetBytes(buffer);

            for (int i = 0; i < pwdLength; i++)
            {
                randomPassword += inputStr[buffer[i] % inputStr.Length];
            }
        }


        PasswordOutputTextBox.Text = randomPassword;
    }
}
