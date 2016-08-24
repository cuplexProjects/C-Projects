using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuplexLib;
using CLinq = CuplexLib.Linq;

public partial class UserControls_PollControl : System.Web.UI.UserControl
{
    public bool HasVoted { get; private set; }   
    protected void Page_Load(object sender, EventArgs e)
    {
        PollPanel.Visible = this.Visible;
    }
    protected override void OnPreRender(EventArgs e)
    {
        User user = Session["User"] as User;
        registerScripts();
        PollPanel.Controls.Clear();

        using (var db = CLinq.DataContext.Create())
        {
            //Get last active poll
            CLinq.Poll poll = db.Polls.Where(p => p.IsActive).OrderByDescending(p => p.CreateDate).Take(1).SingleOrDefault();

            if (poll == null)
            {
                this.Visible = false;
                return;
            }
            else
                this.Visible = true;

            if (user != null)
                this.HasVoted = db.UserToPollOptions.Any(x => x.UserRef == user.UserRef && x.PollOption.PollRef == poll.PollRef);

            Label pollDescription = new Label();
            pollDescription.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
            pollDescription.Style.Add(HtmlTextWriterStyle.FontSize, "1em");
            pollDescription.Text = poll.Description;
            PollPanel.Controls.Add(pollDescription);

            Table pollTable = new Table();
            pollTable.ID = "PollTable";
            pollTable.CellPadding = 0;
            pollTable.CellSpacing = 0;
            pollTable.CssClass = "pollTable";

            List<CLinq.PollOption> pollOptionList = poll.PollOptions.OrderBy(x => x.SortOrder).ThenBy(x => x.OptionName).ToList();

            foreach (var po in pollOptionList)
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                RadioButton pollRadioButton = new RadioButton();
                pollRadioButton.Attributes.Add("poRef", po.PollOptionRef.ToString());
                pollRadioButton.Text = po.OptionName;
                pollRadioButton.GroupName = "Poll";
                cell.Controls.Add(pollRadioButton);

                row.Cells.Add(cell);
                pollTable.Rows.Add(row);
            }

            
            PollPanel.Controls.Add(pollTable);

            Button voteButton = new Button();
            voteButton.CssClass = "pollButton";
            if (user == null)
                voteButton.OnClientClick = "alert('Du måste vara inloggad för att rösta');return false;";
            else
                voteButton.OnClientClick = "PollVote('" + pollTable.ClientID + "');return false;";
            voteButton.Text = "Rösta";

            Button pollResultButton = new Button();
            pollResultButton.CssClass = "pollButton";
            pollResultButton.OnClientClick = "ShowPollResult(" + poll.PollRef + ");return false;";
            pollResultButton.Text = "Se resultat";
            
            PollPanel.Controls.Add(voteButton);
            PollPanel.Controls.Add(pollResultButton);
        }
    }
    private void registerScripts()
    {

    }
}
