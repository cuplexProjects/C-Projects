using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using CLinq = CuplexLib.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CuplexLib;

/// <summary>
/// Summary description for ContentList
/// </summary>
public class ContentList : Panel
{
    public DateTime LinkDataListDate { get; private set; }
    private List<LinkDataItem> linkDataItemList = null;
    private bool IsDisplayingSearchResult = false;
    private string SearchString = "";

    public ContentList()
    {
        linkDataItemList = new List<LinkDataItem>();
        LinkDataListDate = DateTime.MinValue;
    }

    public void LoadData(DateTime linkDate)
    {
        CuplexLib.User user = HttpContext.Current.Session["User"] as CuplexLib.User;
        LinkDataListDate = linkDate;
        using (CLinq.DataContext db = CLinq.DataContext.Create())
        {
            var linkQuery =
            from lnk in db.Links
            where lnk.LinkDate.Date == linkDate
            orderby lnk.LinkDate descending
            select lnk;

            List<CLinq.Link> linkList = linkQuery.ToList();
            foreach (var lnk in linkList)
            {
                LinkDataItem linkDataItem = new LinkDataItem();
                linkDataItem.LinkRef = lnk.LinkRef;
                linkDataItem.Category = lnk.Category.CategoryName;
                linkDataItem.Clicks = lnk.Clicks;
                linkDataItem.LinkName = lnk.LinkName;
                linkDataItem.LinkUrl = lnk.LinkUrl;
                linkDataItem.Rating = lnk.Rating;
                if (lnk.UserRef != null)
                    linkDataItem.UserName = lnk.User.UserName;

                if (user != null)
                    linkDataItem.HasVoted = db.UserToLinks.Where(utl => utl.LinkRef == lnk.LinkRef && utl.UserRef == user.UserRef).Any();

                linkDataItem.NumberOfComments = lnk.Comments.Count;

                linkDataItemList.Add(linkDataItem);
            }
        }
    }
    public void LoadDataFromSearch(string searchText, List<CLinq.Link> linkList, CLinq.DataContext db)
    {
        CuplexLib.User user = HttpContext.Current.Session["User"] as CuplexLib.User;

        foreach (var lnk in linkList)
        {
            LinkDataItem linkDataItem = new LinkDataItem();
            linkDataItem.LinkRef = lnk.LinkRef;
            linkDataItem.Category = lnk.Category.CategoryName;
            linkDataItem.Clicks = lnk.Clicks;
            linkDataItem.LinkName = lnk.LinkName;
            linkDataItem.LinkUrl = lnk.LinkUrl;
            linkDataItem.Rating = lnk.Rating;
            if (lnk.UserRef != null)
                linkDataItem.UserName = lnk.User.UserName;

            if (user != null)
                linkDataItem.HasVoted = db.UserToLinks.Where(utl => utl.LinkRef == lnk.LinkRef && utl.UserRef == user.UserRef).Any();

            linkDataItem.NumberOfComments = lnk.Comments.Count;

            linkDataItemList.Add(linkDataItem);
        }
        IsDisplayingSearchResult = true;
        SearchString = searchText;
    }

    protected override void OnPreRender(EventArgs e)
    {
        CuplexLib.User user = HttpContext.Current.Session["User"] as CuplexLib.User;
        Table contentTable = new Table();
        contentTable.CellPadding = 0;
        contentTable.CellSpacing = 0;
        contentTable.CssClass = "contentList";
        TableHeaderRow thr = new TableHeaderRow();
        TableHeaderCell thc = new TableHeaderCell();
        thc.ColumnSpan = 3;
        if (IsDisplayingSearchResult)
            thc.Text = "Sökresultat för: " + SearchString;
        else
            thc.Text = Utils.GetResourceText("WeekDay" + (int)LinkDataListDate.DayOfWeek) + " " + DateHandler.ToDateString(LinkDataListDate);
        thr.Cells.Add(thc);
        contentTable.Rows.Add(thr);
        int rowCnt = 0;
        string baseUrl = cms.Current.GetRootPath;

        foreach (LinkDataItem linkDataItem in linkDataItemList)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();

            //Column1
            HtmlGenericControl theLink = new HtmlGenericControl("a");
            theLink.InnerHtml = HttpContext.Current.Server.HtmlEncode(linkDataItem.LinkName);
            theLink.Attributes.Add("href", baseUrl + "go/" + linkDataItem.LinkRef);
            theLink.Attributes.Add("target", "_blank");


            Table innerTable = new Table();
            innerTable.CellSpacing = 0;
            innerTable.CellPadding = 0;            

            TableRow innerTableRow = new TableRow();
            TableCell innerTableCell = new TableCell();
            innerTableCell.ColumnSpan = 3;

            innerTableCell.CssClass = "link";
            innerTableCell.Controls.Add(theLink);
            innerTableRow.Cells.Add(innerTableCell);
            innerTable.Rows.Add(innerTableRow);

            innerTableRow = new TableRow();
            innerTableCell = new TableCell();
            innerTableCell.CssClass = "innerTableCell";
            innerTableCell.Text = "Kategori: " + linkDataItem.Category;
            innerTableRow.Cells.Add(innerTableCell);

            innerTableCell = new TableCell();
            innerTableCell.CssClass = "innerTableCell";
            innerTableCell.Text = "Klick: " + linkDataItem.Clicks;
            innerTableRow.Cells.Add(innerTableCell);

            innerTableCell = new TableCell();
            innerTableCell.CssClass = "innerTableCell";
            innerTableCell.Text = "Tipsare: " + linkDataItem.UserName;
            innerTableRow.Cells.Add(innerTableCell);

            TableRow subTableRow = new TableRow();
            TableCell subTableCell = new TableCell();
            Table subTable = new Table();
            subTable.CellPadding = 0;
            subTable.CellSpacing = 0;
            subTable.Style.Add(HtmlTextWriterStyle.Margin, "0");
            subTable.Rows.Add(innerTableRow);

            subTableCell.Controls.Add(subTable);
            subTableRow.Cells.Add(subTableCell);

            
            innerTable.Rows.Add(subTableRow);

            td.Controls.Add(innerTable);
            tr.Cells.Add(td);

            //Column2
            td = new TableCell();
            //td.Text = "Betyg här!";
            StarVotingControl votingControl = new StarVotingControl();
            votingControl.LinkRef = linkDataItem.LinkRef;
            votingControl.Rating = linkDataItem.Rating;
            votingControl.HasVoted = linkDataItem.HasVoted;
            if (user != null)
                votingControl.UserRef = user.UserRef;
            td.Controls.Add(votingControl);

            td.CssClass = "votingTd";
            tr.Cells.Add(td);

            //Column3
            td = new TableCell();
            td.CssClass = "rightColumn commentTd";
            HtmlGenericControl commentLink = new HtmlGenericControl("a");
            commentLink.Attributes.Add("class", "commentLink");
            commentLink.Attributes.Add("href", "#");
            if (user != null)
                commentLink.Attributes.Add("onclick", "javascript:ShowPopup('" + cms.Current.GetRootPath + "Comment.aspx?linkId=" + linkDataItem.LinkRef + "',530,440);return false;");
            else
                commentLink.Attributes.Add("onclick", "javascript:ShowPopup('" + cms.Current.GetRootPath + "Comment.aspx?linkId=" + linkDataItem.LinkRef + "',530,315);return false;");
            commentLink.InnerHtml = linkDataItem.NumberOfComments.ToString() + " inlägg";
            td.Controls.Add(commentLink);
             
            tr.Cells.Add(td);

            if (rowCnt % 2 == 1)
                tr.CssClass = "listRow";
            else
                tr.CssClass = "listRowAlt";

            rowCnt++;
            contentTable.Rows.Add(tr);
        }

        this.CssClass = "contentWrapper dropShadowSoft";
        this.Controls.Add(contentTable);
        base.OnPreRender(e);
    }

    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }


    private class LinkDataItem
    {
        public int LinkRef { get; set; }
        public string LinkName { get; set; }
        public string LinkUrl { get; set; }
        public string Category { get; set; }
        public double Rating { get; set; }
        public int Clicks { get; set; }
        public string UserName { get; set; }
        public int NumberOfComments { get; set; }
        public bool HasVoted { get; set; }
    }
}
