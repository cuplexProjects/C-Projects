using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CuplexLib;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for PollResultRender
/// </summary>
public class PollResultRender
{
    public PollResultRender()
    {
    }

    public static string RenderPollResult(PollResult pollResult)
    {
        int totalRatings = 0;
        System.IO.StringWriter stringWriter = new System.IO.StringWriter();
        HtmlTextWriter writer = new HtmlTextWriter(stringWriter);

        HtmlGenericControl divWrapper = new HtmlGenericControl("div");
        divWrapper.ID = "pollContentWrapper";

        HtmlGenericControl pollHeader = new HtmlGenericControl("p");
        pollHeader.Attributes.Add("class", "pollDescription");
        pollHeader.InnerHtml = pollResult.PollDescription;

        divWrapper.Controls.Add(pollHeader);

        Table pollResultTable = new Table();
        pollResultTable.CssClass = "pollResultTable";
        pollResultTable.CellPadding = 0;
        pollResultTable.CellSpacing = 0;

        divWrapper.Controls.Add(pollResultTable);

        foreach (PollResultOption pro in pollResult.PollResultOptionList)
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Text = pro.PollOptionName;
            row.Cells.Add(cell);
            pollResultTable.Rows.Add(row);

            row = new TableRow();
            cell = new TableCell();
            HtmlGenericControl pbarDivWrapper = new HtmlGenericControl("div");
            pbarDivWrapper.Attributes.Add("class", "pbarWrapper");

            HtmlGenericControl pbarDiv = new HtmlGenericControl("div");
            pbarDiv.Attributes.Add("class", "pollPbar");
            pbarDiv.Style.Add(HtmlTextWriterStyle.Width, Math.Round((pro.Rating * 250d), 0).ToString() + "px");

            pbarDivWrapper.Controls.Add(pbarDiv);
            
            HtmlGenericControl pbarDescr = new HtmlGenericControl("strong");
            pbarDescr.InnerHtml = Math.Round(pro.Rating * 100, 0) + "%" + " (" + pro.Ratings + ")";

            pbarDivWrapper.Controls.Add(pbarDescr);
            cell.Controls.Add(pbarDivWrapper);
            

            row.Cells.Add(cell);
            pollResultTable.Rows.Add(row);

            totalRatings += pro.Ratings;
        }


        HtmlGenericControl totalRatingsLabel = new HtmlGenericControl("div");
        totalRatingsLabel.InnerHtml = "Totalt antal röster: <strong>" + totalRatings + "</strong>";
        totalRatingsLabel.Style.Add(HtmlTextWriterStyle.MarginTop, "10px");
        divWrapper.Controls.Add(totalRatingsLabel);

        divWrapper.RenderControl(writer);
        writer.Flush();
        return stringWriter.ToString();
    }
}