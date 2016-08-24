using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FileGetDbLib;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AdminPanel.Visible = Request.IsLocal;
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

    }

    protected void GenerateButton_click(object sender, EventArgs e)
    {
        if (FilePathTextBox.Text.Length > 0)
        {
            FileShareMain fm = new FileShareMain();
            bool isDirectory = !System.IO.Path.HasExtension(FilePathTextBox.Text); //CheckIfIsDir(FilePathTextBox.Text);
            try
            {
                if (!isDirectory && !(new System.IO.FileInfo(FilePathTextBox.Text).Exists))
                {
                    ErrorLabel.Text = "invalid path";
                    return;
                }
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                string linkId = fm.CreateLocalFileLink(FilePathTextBox.Text, isDirectory);
                urlTextBox.Text = baseUrl + "Download.aspx?id=" + linkId;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
            }

        }
    }

    private bool CheckIfIsDir(string path)
    {
        for (int i = path.Length - 1; i > 0; i--)
        {
            if (path[i]=='\\')
                return true;
            else if (path[i]=='.')
                return false;
        }
        return false;
    }
}