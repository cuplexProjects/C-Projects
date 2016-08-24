using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuplexLib;

public partial class TestPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StarVotingPanel.Controls.Add(new StarVotingControl());

    }
    protected override void OnPreRender(EventArgs e)
    {
        //TestLabel.Text = PasswordReset.GetRandomString(32);
        TestLabel.Text = Utils.GetResourceText("WeekDay"+((int)DateTime.Today.DayOfWeek).ToString());
    }
    protected void TransferButton_Clicked(object sender, EventArgs e)
    {
        
    }
    protected void LookupIpButton_Clicked(object sender, EventArgs e)
    {
        IpLookup ipl = IpLookup.LookupIp(LookupIpTextBox.Text);
        if (ipl == null) IpLookupResultLabel.Text = "Invalid ip number";
        else
            IpLookupResultLabel.Text = ipl.Country;
    }
}
