using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuplexLib;

public partial class UserControls_SearchControl : System.Web.UI.UserControl
{
    private string searchForLinkText;
    protected void Page_Load(object sender, EventArgs e)
    {
        searchForLinkText = Utils.GetResourceText("SearchForLinkText");
        if (!this.IsPostBack)
        {
            CategoryDropdownList.DataSource = Category.GetCategoryList();
            CategoryDropdownList.DataTextField = "CategoryName";
            CategoryDropdownList.DataValueField = "CategoryRef";
            CategoryDropdownList.DataBind();

            CategoryDropdownList.Items.Insert(0, new ListItem("Kategori", "0"));
        }

        //this.Page.Form.Action = cms.Current.GetRootPath + "Search";
    }
    protected override void OnPreRender(EventArgs e)
    {
        SearchTextBox.Text = searchForLinkText;
    }
    protected void SearchButton_Clicked(object sender, EventArgs e)
    {
        int catogoryRef = int.Parse(CategoryDropdownList.SelectedValue);
        string searchString = Utils.TruncateString(SearchTextBox.Text, 50);

        if (searchString == searchForLinkText)
            searchString = "";

        ContentList contentList = new ContentList();
        using (var db = CuplexLib.Linq.DataContext.Create())
        {
            var linkQuery =
            from l in db.Links
            where catogoryRef == 0 || l.CategoryRef == catogoryRef
            where searchString == "" || l.LinkName.Contains(searchString)
            orderby l.LinkDate descending
            select l;

            contentList.LoadDataFromSearch(searchString, linkQuery.Take(50).ToList(), db);

            Session["SearchData"] = contentList;
            Response.Redirect(cms.Current.GetRootPath + "search/");
        }
    }
}