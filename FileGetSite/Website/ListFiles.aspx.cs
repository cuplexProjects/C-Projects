using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FileGetDbLib;
using FileGetDbLib.Linq;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Collections;

public partial class ListFilesPage : System.Web.UI.Page, IPostBackEventHandler
{
    private const string HASH_SALT = "lf2V5bRvyLl7EH2DtvoUY8Cpy4YujdFK";
    private const string HASH_SALT2 = "htXRmdoyKb4FMWXXuHsdVDbGx5YgKdcW";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public void RaisePostBackEvent(string eventArgument)
    {

        ErrorMessageLabel.Text = "ok";
    }

    protected override void OnPreRender(EventArgs e)
    {
        string linkId = Request.QueryString["id"];
        if (linkId == null)
        {
            ErrorMessageLabel.Text = "Invalid link";
            return;
        }

        FileShareMain fm = new FileShareMain();

        try
        {
            FileShare fs = fm.GetLocalFilePath(linkId);
            if (fs != null)
            {
                Session["IsAuthorized"] = true;
                Session["linkId"] = linkId;

                if (fs.IsDirectory)
                {
                    Hashtable htFiles = new Hashtable();
                    System.IO.FileInfo[] files = new System.IO.DirectoryInfo(fs.FilePath).GetFiles();
                    FileLinkPanel.Controls.Clear();
                    HtmlGenericControl ul = new HtmlGenericControl("ul");
                    FileLinkPanel.Controls.Add(ul);

                    foreach (System.IO.FileInfo fi in files)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        HtmlAnchor lnk = new HtmlAnchor();
                        lnk.InnerText = fi.Name;

                        string linkHashCode = SHA2Hash.GetSHA2Hash(SHA2Hash.HashBits.n256, HASH_SALT + fi.Name + HASH_SALT2);
                        lnk.HRef = "download/" + linkHashCode;

                        //Add record to hashtable
                        htFiles.Add(linkHashCode, fi.FullName);

                        li.Controls.Add(lnk);
                        ul.Controls.Add(li);
                    }
                    HttpRuntime.Cache.Insert("AvailableFileHashTable_" + linkId, htFiles, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.NotRemovable, null);            
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessageLabel.Text = ex.Message;
        }
        base.OnPreRender(e);
    }
}