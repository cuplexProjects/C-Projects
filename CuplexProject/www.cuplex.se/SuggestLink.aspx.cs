using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuplexLib;

public partial class SuggestLink : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
            Response.Redirect(cms.Current.GetRootPath);

        if (!this.IsPostBack)
        {
            CategoryDropdownList.DataValueField = "CategoryRef";
            CategoryDropdownList.DataTextField = "CategoryName";
            CategoryDropdownList.DataSource = Category.GetCategoryList();
            CategoryDropdownList.DataBind();
        }
    }
    private bool ValidateBeforeSave()
    {
        if (string.IsNullOrEmpty(DescriptionTextBox.Text))
        {
            base.ShowModalMessage("Du måste ange en beskrivning");
            return false;
        }
        else if (string.IsNullOrEmpty(UrlTextBox.Text))
        {
            base.ShowModalMessage("Du måste ange en Url");
            return false;
        }
        else
            return true;
    }
    protected void SuggestLinkButton_Clicked(object sender, EventArgs e)
    {
        CuplexLib.User user = Session["User"] as CuplexLib.User;
        if (user != null && ValidateBeforeSave())
        {
            LinkSuggestion link = new LinkSuggestion();
            link.UserRef = user.UserRef;
            link.CategoryRef = int.Parse(CategoryDropdownList.SelectedValue);
            link.Description = Utils.TruncateString(DescriptionTextBox.Text, 250);
            link.LinkUrl = Utils.TruncateString(UrlTextBox.Text, 500);
            link.LinkSuggestionDate = DateTime.Now;
            link.Save();

            DescriptionTextBox.Text = "";
            UrlTextBox.Text = "";
            Label messageLabel = new Label();
            messageLabel.Text = "Länken är nu postad";
            LinkSuggestMessagePanel.Visible = true;
            LinkSuggestMessagePanel.Controls.Add(messageLabel);
        }
    }
}
