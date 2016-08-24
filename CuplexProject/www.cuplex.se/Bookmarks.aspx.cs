using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using CuplexLib;

public partial class Bookmarks : BasePage
{
    private const int MAX_UPLOAD_SIZE = 1048576;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
        {
            NotLoggedInPanel.Visible = true;
            LoggedInPanel.Visible = false;
        }
        else
        {
            NotLoggedInPanel.Visible = false;
            LoggedInPanel.Visible = true;
            UploadButton.Text = Utils.GetResourceText("UploadButton");
            RegularExpressionValidator1.ErrorMessage = Utils.GetResourceText("BookmarksRegularExpressionValidatorText");
        }
    }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }
    protected void UploadButton_Clicked(object sender, EventArgs e)
    {
        CuplexLib.User user = Session["User"] as CuplexLib.User;
        if (FileUploadControl.HasFile)
        {
            try
            {
                if (FileUploadControl.PostedFile.ContentLength > MAX_UPLOAD_SIZE)
                {
                    ShowModalMessage(Utils.GetResourceText("ToLargeUploadFileText"));
                    return;
                }
                saveUploadFile();
                Bookmark.ParseJsonBookmarkFile(user.UserRef, cms.Current.LocalFilePath + "Upload\\" + user.UserRef + ".json");
            }
            catch (Exception ex)
            {
                EventLog.SaveToEventLog(ex.Message, EventLogType.Error, user.UserRef);
            }
        }
        else
        {
            ShowModalMessage(Utils.GetResourceText("NoFileSelectedText"));
        }
    }
    private void saveUploadFile()
    {
        CuplexLib.User user = Session["User"] as CuplexLib.User;
        if (user == null) return;

        string fileName = cms.Current.LocalFilePath + "Upload\\" + user.UserRef + ".json";
        try
        {
            FileStream fs = File.Create(fileName);
            byte[] buffer = new byte[102400];
            int bytesRead = 0;

            do
            {
                bytesRead = FileUploadControl.FileContent.Read(buffer, 0, buffer.Length);
                fs.Write(buffer, 0, bytesRead);
            }
            while (bytesRead > 0);

            fs.Close();            
        }
        catch (Exception ex)
        {
            EventLog.SaveToEventLog(ex.Message, EventLogType.Error, user.UserRef);
        }
    }
}