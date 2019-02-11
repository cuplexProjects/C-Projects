using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CLinq = CuplexLib.Linq;

public partial class SiteLinks : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected override void OnPreRender(EventArgs e)
    {
        using(var db = CLinq.DataContext.Create())
        {
            List<CLinq.SiteLink> siteLinkList = db.SiteLinks.Where(s => s.IsActive).ToList();

            Table siteLinkTable = new Table();
            siteLinkTable.CssClass = "siteLinkTable";
            SiteLinkPanel.Controls.Add(siteLinkTable);

            foreach (var siteLink in siteLinkList)
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();

                HyperLink link = new HyperLink();
                link.Target = "_blank";
                link.Text = siteLink.SiteLinkName;
                link.NavigateUrl = siteLink.SiteLinkURL;
                cell.Controls.Add(link);
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = siteLink.Description;
                row.Cells.Add(cell);

                siteLinkTable.Rows.Add(row);
            }            
        }
        base.OnPreRender(e);
    }
}