using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuplexLib;
using System.Web.UI.HtmlControls;


public partial class CommentPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] is CuplexLib.User)
            PostCommentPanel.Visible = true;
    }
    protected override void OnPreRender(EventArgs e)
    {
        int linkRef;
        if (!string.IsNullOrEmpty(Request.QueryString["linkId"]) && int.TryParse(Request.QueryString["linkId"], out linkRef))
        {
            CreateLinkTable(linkRef);
            this.LinkRef = linkRef;
        }
    }

    public void CreateLinkTable(int linkRef)
    {
        List<Comment> commentList = Comment.GetCommentListForLink(linkRef);

        Table commentTable = new Table();
        commentTable.CellPadding = 0;
        commentTable.CellSpacing = 0;
        commentTable.CssClass = "CommentTable";
        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        foreach (Comment c in commentList)
        {
            Panel headerPanel = new Panel();
            HtmlGenericControl commentText = new HtmlGenericControl("div");
            Label dateLabel = new Label();
            Label userNameLabel = new Label();

            userNameLabel.Text = c.UserName;
            userNameLabel.CssClass = "commentUser";
            dateLabel.Text = DateHandler.ToDateString(c.CommentDate) + " " + DateHandler.ToTimeString(c.CommentDate);
            dateLabel.CssClass = "commentDate";

            commentText.InnerHtml = c.CommentText;

            headerPanel.Controls.Add(userNameLabel);
            headerPanel.Controls.Add(dateLabel);

            td.Controls.Add(headerPanel);
            tr.Cells.Add(td);
            commentTable.Rows.Add(tr);

            tr = new TableRow();
            td = new TableCell();
            td.Controls.Add(commentText);
            tr.Cells.Add(td);
            commentTable.Rows.Add(tr);

            Panel dividerPanel = new Panel();
            dividerPanel.CssClass = "divider";
            tr = new TableRow();
            td = new TableCell();
            td.Controls.Add(dividerPanel);
            tr.Cells.Add(td);
            commentTable.Rows.Add(tr);
        }

        if (commentTable.Rows.Count > 0)
            commentTable.Rows.Remove(commentTable.Rows[commentTable.Rows.Count - 1]);
        else
        {
            Panel fillPanel = new Panel();
            Label fillLabel = new Label();
            fillLabel.Text = "&nbsp;";
            fillPanel.Controls.Add(fillLabel);

            td.Controls.Add(fillPanel);
            tr.Cells.Add(td);
            commentTable.Rows.Add(tr);
        }

        Panel tableWrapper = new Panel();
        tableWrapper.CssClass = "CommentTableWrapper";
        tableWrapper.Controls.Add(commentTable);

        CommentPanel.Controls.Add(tableWrapper);
    }
    protected void PostCommentButton_Clicked(object sender, EventArgs e)
    {
        CuplexLib.User user = Session["User"] as CuplexLib.User;
        if (user == null || LinkRef == 0 || CommentTextBox.Text.Length == 0)
            return;

        Comment comment = new Comment();
        comment.CommentDate = DateTime.Now;
        comment.CommentText = Utils.TruncateString(CommentTextBox.Text, 5000);
        comment.LinkRef = this.LinkRef;
        comment.UserRef = user.UserRef;

        comment.Save();
        CommentTextBox.Text = "";
    }

    public int LinkRef 
    {
        get
        {            
            if (this.ViewState["LinkRef"] is int)
                return (int)this.ViewState["LinkRef"];
            else
                return 0;
        }
        set { this.ViewState["LinkRef"] = value; }
    }
}
