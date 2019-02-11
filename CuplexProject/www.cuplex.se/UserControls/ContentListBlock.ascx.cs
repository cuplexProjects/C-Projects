using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuplexLib;
using CLinq = CuplexLib.Linq;
using System.Web.UI.HtmlControls;

public partial class ContentListBlock : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected override void OnPreRender(EventArgs e)
    {
        if (Session["SearchData"] is ContentList)
        {
            ContentList searchContentList = Session["SearchData"] as ContentList;
            this.Controls.Clear();
            this.Controls.Add(searchContentList);
            Session["SearchData"] = null;
            return;
        }

        int pageId = 1;
        List<DateTime> linkDateList = new List<DateTime>();

        //Try Parse PageId
        if (!string.IsNullOrEmpty(Request.QueryString["pageId"]))
        {
            int.TryParse(Request.QueryString["pageId"], out pageId);
            if (pageId < 1) pageId = 1;
        }

        
        using (CLinq.DataContext db = CLinq.DataContext.Create())
        {
            var dateQuery =
            from l in db.Links
            group l by l.LinkDate.Date into lg
            orderby lg.Key descending
            select lg.Key;

            int maxPage = Math.Max(dateQuery.Count() / 3, 1) + 1;

            if (pageId == 1)
                linkDateList = dateQuery.Take(3).ToList();
            else if (pageId > maxPage)
                Response.Redirect(cms.Current.GetRootPath);
            else
            {
                linkDateList = dateQuery.Take(3 * pageId).Skip((3 * pageId) - 3).ToList();
            }
        }

        this.Controls.Clear();
        foreach (DateTime linkDate in linkDateList)
        {
            ContentList contentList = new ContentList();            
            contentList.LoadData(linkDate);
            this.Controls.Add(contentList);
        }

        if (this.ShowPageLinks)
        {
            if (pageId > 1)
                this.Controls.Add(GetNavigationLink(false, pageId));
            this.Controls.Add(GetNavigationLink(true, pageId));
        }
    }
    private HtmlGenericControl GetNavigationLink(bool next, int pageId)
    {
        HtmlGenericControl divWrapper = new HtmlGenericControl("div");
        HtmlGenericControl navigationLink = new HtmlGenericControl("a");
        divWrapper.Controls.Add(navigationLink);

        divWrapper.Attributes.Add("class", next ? "nextLink" : "prevLink");
        divWrapper.Attributes["class"] += " link";
        navigationLink.InnerHtml = next ? "Nästa sida»" : "«Föregående sida";
        if (next)
            navigationLink.Attributes.Add("href", cms.Current.GetRootPath + "page/" + (pageId + 1));
        else
            navigationLink.Attributes.Add("href", cms.Current.GetRootPath + "page/" + (pageId - 1));

        return divWrapper;
    }

    #region Properties
    public int CacheTime { get; set; }  //In Minutes
    public bool ShowPageLinks { get; set; }
    public int PageNumbertt
    {
        get 
        {
            if (ViewState["PageNumber"] is int)
                return (int)ViewState["PageNumber"];
            else
            {
                ViewState["PageNumber"] = 1;
                return 1;
            }
        }
        private set { ViewState["PageNumber"] = value; }
    }
    #endregion
}
