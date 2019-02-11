using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuplexLib;
using CLinq = CuplexLib.Linq;

public partial class AdminPage : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CuplexLib.User user = Session["User"] as CuplexLib.User;
        if (user == null || !user.IsAdmin)
            Server.Transfer("StartPage.aspx");
    }
    protected override void OnPreRender(EventArgs e)
    {
        CuplexLib.User user = Session["User"] as CuplexLib.User;
        if (user == null || !user.IsAdmin)
            return;

        AdminPagePanel.Visible = true;
        registerScripts();
        if (!IsPostBack)
        {
            FromDateTextBox.Text = DateHandler.ToDateString(DateTime.Today.AddDays(-3));
            UntilDateTextBox.Text = DateHandler.ToDateString(DateTime.Today);
            loadDataList();
        }
        
        base.OnPreRender(e);
    }
    protected void LinkSuggestionButton_Clicked(object sender, EventArgs e)
    {
        bool hasChanged = this.SelectedView != ViewMode.LinkSuggestion;
        this.SelectedView = ViewMode.LinkSuggestion;
        if (hasChanged)
            loadDataList();
    }
    protected void LinkAdministrationButton_Clicked(object sender, EventArgs e)
    {
        bool hasChanged = this.SelectedView != ViewMode.LinkAdministration;
        this.SelectedView = ViewMode.LinkAdministration;
        if (hasChanged)
            loadDataList();
    }
    protected void UserAdministrationButton_Clicked(object sender, EventArgs e)
    {
        bool hasChanged = this.SelectedView != ViewMode.UserAdministration;
        this.SelectedView = ViewMode.UserAdministration;
        if (hasChanged)
            loadDataList();
    }
    protected void UpdateFileListButton_Clicked(object sender, EventArgs e)
    {
        RandomImage.UpdateDB(WebUtils.GetRandomImgFileList());

        //Empty Runtime cache
        HttpRuntime.Cache.Remove("randomImageList");
        HttpRuntime.Cache.Remove("RandomFileList");
        HttpContext.Current.Session.Remove("RandomImageSequence");
    }
    protected void LoadDataButton_Clicked(object sender, EventArgs e)
    {
        loadDataList();
    }
    private void loadDataList()
    {
        LinkSuggestionList.Visible = false;
        LinkList.Visible = false;
        UserList.Visible = false;
        CreateLinksButton.Visible = false;
        AddLinkButton.Visible = false;
        AdminUpperPanel.Visible = false;

        switch (SelectedView)
        {
            case ViewMode.LinkSuggestion:
                LinkSuggestionList.Visible = true;
                CreateLinksButton.Visible = true;
                AdminUpperPanel.Visible = true;
                LoadLinkSuggestionList();
                break;
            case ViewMode.LinkAdministration:
                AddLinkButton.Visible = true;
                LinkList.Visible = true;
                AdminUpperPanel.Visible = true;
                LoadLinkList();
                break;
            case ViewMode.UserAdministration:
                UserList.Visible = true;
                LoadUserList();
                break;
        }
    }

    private void LoadLinkList()
    {
        DateTime fromDate;
        DateTime untilDate;
        List<int> selectedLinkRefList = GetPostCollection("LinkSelectionCheckBox");
        try
        {
            fromDate = DateHandler.ParseDateString(FromDateTextBox.Text);
            untilDate = DateHandler.ParseDateString(UntilDateTextBox.Text);
        }
        catch { return; }

        using (var db = CLinq.DataContext.Create())
        {
            var linkQuery =
            from li in db.Links
            where li.LinkDate.Date >= fromDate && li.LinkDate.Date <= untilDate
            orderby li.LinkDate descending
            select new
            {
                li.LinkRef,
                li.LinkName,
                li.LinkDate,
                li.CategoryRef,
                CategoryName = li.Category.CategoryName,
                UserName = li.UserRef != null ? li.User.UserName : "",
                li.LinkUrl,
                li.Clicks,
                li.Rating,
                IsChecked = selectedLinkRefList.Contains(li.LinkRef)
            };

            LinkList.DataSource = linkQuery.ToList();
            LinkList.DataBind();

        }
    }

    protected void LoadLinkSuggestionList()
    {
        DateTime fromDate;
        DateTime untilDate;
        List<int> linkSuggestionRefList = GetPostCollection("SuggestionCheckBox");
        try
        {
            fromDate = DateHandler.ParseDateString(FromDateTextBox.Text);
            untilDate = DateHandler.ParseDateString(UntilDateTextBox.Text);
        }
        catch { return; }
        using (var db = CLinq.DataContext.Create())
        {
            var linkSuggestionQuery =
            from ls in db.LinkSuggestions
            where ls.LinkSuggestionDate.Date <= untilDate && ls.LinkSuggestionDate.Date >= fromDate
            select new
            {
                ls.LinkSuggestionRef,
                ls.LinkUrl,
                ls.Description,
                ls.Category.CategoryName,
                ls.User.UserName,
                ls.LinkSuggestionDate,
                IsChecked = linkSuggestionRefList.Contains(ls.LinkSuggestionRef)
            };
            
            LinkSuggestionList.DataSource = linkSuggestionQuery.ToList(); ;
            LinkSuggestionList.DataBind();
        }
    }
    protected void LoadUserList()
    {
        using (var db = CLinq.DataContext.Create())
        {
            var userQuery =
            from u in db.Users
            orderby u.UserRef
            select new
            {
                u.UserRef,
                u.UserName,
                u.EmailAddress,
                LastLoginDate = u.LastLogin,
                Comments = u.Comments.Count,
                Ratings = u.UserToLinks.Count,
                PollVotes = u.UserToPollOptions.Count,
                u.IsAdmin
            };

            UserList.DataSource = userQuery.ToList();
            UserList.DataBind();
        }        
    }
    protected void LinkList_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
    }
    protected void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (SelectedView == ViewMode.LinkSuggestion)
        {
            List<int> linkSuggestionRefList = GetPostCollection("SuggestionCheckBox");
            LinkSuggestion.DeleteMany(linkSuggestionRefList);
        }
        else if (SelectedView == ViewMode.LinkAdministration)
        {
            List<int> linkSuggestionRefList = GetPostCollection("LinkSelectionCheckBox");
            Link.DeleteMany(linkSuggestionRefList);
        }
    }
    protected void EditButton_Clicked(object sender, EventArgs e)
    {
        CuplexLib.User user = Session["User"] as CuplexLib.User;
        if (SelectedView == ViewMode.LinkSuggestion)
        {
            List<int> linkSuggestionRefList = GetPostCollection("SuggestionCheckBox");
            if (linkSuggestionRefList.Count > 0)
            {
                LinkSuggestion linkSuggestion = LinkSuggestion.GetOne(linkSuggestionRefList[0]);
                if (linkSuggestion == null) return;

                LinkSuggestionDescriptionTextBox.Text = linkSuggestion.Description;
                LinkSuggestionUrlTextBox.Text = linkSuggestion.LinkUrl;

                LinkSuggestionCategoryDropdownList.DataValueField = "CategoryRef";
                LinkSuggestionCategoryDropdownList.DataTextField = "CategoryName";
                LinkSuggestionCategoryDropdownList.DataSource = Category.GetCategoryList();
                LinkSuggestionCategoryDropdownList.DataBind();

                try { LinkSuggestionCategoryDropdownList.SelectedValue = linkSuggestion.CategoryRef.ToString(); }
                catch { }

                this.ViewState["EditLinkSuggestionRef"] = linkSuggestion.LinkSuggestionRef;
                EditLinkSuggestionPanel.Visible = true;
                base.ShowModalBackground();
            }
        }
        else if (SelectedView == ViewMode.LinkAdministration)
        {
            List<int> selectedLinkRefRefList = GetPostCollection("LinkSelectionCheckBox");
            if (selectedLinkRefRefList.Count > 0)
            {
                Link link = Link.GetOne(selectedLinkRefRefList[0]);
                if (link == null) return;

                LinkDateTextBox.Text = DateHandler.ToDateString(link.LinkDate);
                LinkTimeTextBox.Text = DateHandler.ToTimeString(link.LinkDate);
                LinkNameTextBox.Text = link.LinkName;

                LinkCategoryDropdownList.DataValueField = "CategoryRef";
                LinkCategoryDropdownList.DataTextField = "CategoryName";
                LinkCategoryDropdownList.DataSource = Category.GetCategoryList();
                LinkCategoryDropdownList.DataBind();

                try { LinkCategoryDropdownList.SelectedValue = link.CategoryRef.ToString(); }
                catch { }

                LinkUserDropDownList.Items.Clear();
                LinkUserDropDownList.Items.Add(new ListItem("null", "0"));

                if (link.UserRef != user.UserRef)
                    LinkUserDropDownList.Items.Add(new ListItem(user.UserName, user.UserRef.ToString()));

                if (link.UserRef != null)
                {
                    LinkUserDropDownList.Items.Add(new ListItem(link.UserName, link.UserRef.Value.ToString()));
                    LinkUserDropDownList.SelectedValue = link.UserRef.Value.ToString();
                }

                LinkUrlTextBox.Text = link.LinkUrl;
                LinkClicksTextBox.Text = link.Clicks.ToString();
                LinkRatingTextBox.Text = Math.Round(link.Rating, 2).ToString();

                this.ViewState["EditLinkRef"] = link.LinkRef;
                EditLinkPanel.Visible = true;
                base.ShowModalBackground();
            }
        }
    }
    protected void AddLinkButton_Clicked(object sender, EventArgs e)
    {
        CuplexLib.User user = Session["User"] as CuplexLib.User;
        LinkDateTextBox.Text = DateHandler.ToDateString(DateTime.Today);
        LinkTimeTextBox.Text = DateHandler.ToTimeString(DateTime.Now);
        LinkCategoryDropdownList.DataValueField = "CategoryRef";
        LinkCategoryDropdownList.DataTextField = "CategoryName";
        LinkCategoryDropdownList.DataSource = Category.GetCategoryList();
        LinkCategoryDropdownList.DataBind();

        LinkUserDropDownList.Items.Clear();
        LinkUserDropDownList.Items.Add(new ListItem("null", "0"));
        LinkUserDropDownList.Items.Add(new ListItem(user.UserName, user.UserRef.ToString()));
        LinkUserDropDownList.SelectedIndex = 1;

        LinkNameTextBox.Text = "";
        LinkUrlTextBox.Text = "";
        LinkClicksTextBox.Text = "0";
        LinkRatingTextBox.Text = "0";

        this.ViewState["EditLinkRef"] = 0;
        EditLinkPanel.Visible = true;
        base.ShowModalBackground();
    }
    protected void EditOkButton_Clicked(object sender, EventArgs e)
    {
        if (SelectedView == ViewMode.LinkSuggestion)
        {
            int linkSuggestionRef;
            if (this.ViewState["EditLinkSuggestionRef"] is int)
            {
                linkSuggestionRef = (int)this.ViewState["EditLinkSuggestionRef"];
                LinkSuggestion linkSuggestion = LinkSuggestion.GetOne(linkSuggestionRef);
                linkSuggestion.Description = LinkSuggestionDescriptionTextBox.Text;
                linkSuggestion.LinkUrl = LinkSuggestionUrlTextBox.Text;
                linkSuggestion.CategoryRef = int.Parse(LinkSuggestionCategoryDropdownList.SelectedValue);
                linkSuggestion.Save();

                this.ViewState.Remove("EditLinkSuggestionRef");
            }
        }
        else if (SelectedView == ViewMode.LinkAdministration)
        {
            int linkRef;
            if (this.ViewState["EditLinkRef"] is int)
            {
                linkRef = (int)this.ViewState["EditLinkRef"];
                DateTime linkDate;
                DateTime linkTime;
                int categoryRef, clicks;
                int? userRef = null;
                double rating;

                try
                {
                    linkDate = DateHandler.ParseDateString(LinkDateTextBox.Text);
                    linkTime = DateHandler.ParseTimeString(LinkTimeTextBox.Text);
                    linkDate = linkDate.Add(linkTime.TimeOfDay);

                    categoryRef = int.Parse(LinkCategoryDropdownList.SelectedValue);
                    clicks = int.Parse(LinkClicksTextBox.Text);
                    rating = double.Parse(LinkRatingTextBox.Text);
                    userRef = int.Parse(LinkUserDropDownList.SelectedValue);
                    if (userRef == 0)
                        userRef = null;
                }
                catch { return; }

                Link link;
                if (linkRef > 0)
                    link = Link.GetOne(linkRef);
                else
                    link = new Link();
                if (link == null) return;

                link.CategoryRef = categoryRef;
                link.Clicks = clicks;
                link.LinkDate = linkDate;
                link.LinkName = LinkNameTextBox.Text;
                link.LinkUrl = LinkUrlTextBox.Text;
                link.Rating = rating;
                link.UserRef = userRef;

                link.Save();

                this.ViewState.Remove("EditLinkRef");
            }
        }
        EditLinkSuggestionPanel.Visible = false;
        EditLinkPanel.Visible = false;
        base.HideModalBackground();
    }
    protected void EditCancelButton_Clicked(object sender, EventArgs e)
    {
        EditLinkSuggestionPanel.Visible = false;
        EditLinkPanel.Visible = false;
        base.HideModalBackground();
    }    

    protected void CreateLinks_Clicked(object sender, EventArgs e)
    {
        if (SelectedView == ViewMode.LinkSuggestion)
        {
            List<int> linkSuggestionRefList = GetPostCollection("SuggestionCheckBox");

            LinkSuggestion.CreateLinksFromLinkSuggestionList(linkSuggestionRefList, false);
            LinkSuggestion.DeleteMany(linkSuggestionRefList);
        }
    }
    private List<int> GetPostCollection(string keyWord)
    {
        List<int> refList = new List<int>();
        foreach (string key in Request.Form.Keys)
        {
            if (key.EndsWith(keyWord))
            {
                string valueStr = Request.Form[key];
                int refValue;

                if (int.TryParse(valueStr, out refValue))
                    refList.Add(refValue);
            }
        }

        return refList;
    }
    private void registerScripts()
    {
        string ClientIdForList = "";

        switch (SelectedView)
        {
            case ViewMode.LinkSuggestion:
                ClientIdForList = LinkSuggestionList.ClientID;
                break;
            case ViewMode.LinkAdministration:
                ClientIdForList = LinkList.ClientID;
                break;
            case ViewMode.UserAdministration:
                ClientIdForList = LinkList.ClientID;
                break;
        }
        
        string script = @"$(function() {
$('#" + FromDateTextBox.ClientID + @"').datepicker({dateFormat: 'yy-mm-dd'});});
$(function() {
$('#" + UntilDateTextBox.ClientID + @"').datepicker({dateFormat: 'yy-mm-dd'});});

function SelectAll()
{    
    var theList = $('#" + ClientIdForList + @"');
    
    if (theList.find('input[type=checkbox][checked]').length != theList.find('input[type=checkbox]').length)
        theList.find(':checkbox').attr('checked','checked');
    else
        theList.find(':checkbox').removeAttr('checked');
}
$(document).ready(function() {
    $('.StandardPopup').css({
        'top': $(window).height() / 2 - $('.StandardPopup').height() / 2,
        'left': $(window).width() / 2 - $('.StandardPopup').width() / 2
    });
});";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "AdminPageScripts", script, true);
    }
    protected ViewMode SelectedView
    {
        get
        {
            if (ViewState["SelectedView"] is ViewMode)
                return (ViewMode)ViewState["SelectedView"];
            else
                return ViewMode.LinkSuggestion;
        }
        set
        {
            ViewState["SelectedView"] = value;
        }
    }   

    protected enum ViewMode
    {
        LinkSuggestion = 1,
        LinkAdministration = 2,
        UserAdministration = 3,        
    }
}
