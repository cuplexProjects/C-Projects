using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;
using CuplexLib;

public partial class RandomImagePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PrevButton.Text = Utils.GetResourceText("PrevButton");
        NextButton.Text = Utils.GetResourceText("NextButton");
    }
    protected override void OnPreRender(EventArgs e)
    {
        string queryString=Request.QueryString["img"];
        if (!string.IsNullOrEmpty(queryString))
        {
            RandomImage ri = WebUtils.GetRandomImageListFromCache().Where(x => x.ImageId == queryString).Take(1).SingleOrDefault();
            if (ri != null)
            {
                string qstrImgPath = cms.Current.GetRootPath + "randimg/" + queryString + ";" + cms.Current.GetRootPath + "RandomImg/" + ri.FileName;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RandomImageScript", "var randImgUrl='" + qstrImgPath + "';", true);
            }
        }
    }
    public static string GetRandomImage(bool next)
    {
        string[] fileList = HttpRuntime.Cache["RandomFileList"] as string[];
        RandomSequence rs = HttpContext.Current.Session["RandomImageSequence"] as RandomSequence;
        if (fileList == null)
        {
            fileList = WebUtils.GetRandomImgFileList();
            HttpRuntime.Cache.Insert("RandomFileList", fileList, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        if (rs == null || (rs != null && rs.SequenceLooped))
        {
            rs = RandomSequence.GetRandomSequence(fileList.Length);
            HttpContext.Current.Session["RandomImageSequence"] = rs;
        }

        List<RandomImage> randomImageList = WebUtils.GetRandomImageListFromCache();

        if (!next)
        {
            //rs.GetPrevious();
            //rs.GetPrevious();
        }

        string randomImage = next ? fileList[rs.GetNext()] : fileList[rs.GetPrevious()];
        string imageDirectLink = "";
        if (randomImageList.Any(x => x.FileName == randomImage))
            imageDirectLink = randomImageList.Where(x => x.FileName == randomImage).Single().ImageId;

        return cms.Current.GetRootPath + "randimg/" + imageDirectLink + ";" + cms.Current.GetRootPath + "RandomImg/" + randomImage;
    }
    [WebMethod(EnableSession = true)]
    public static string GetRandomImage()
    {
        return GetRandomImage(true);
    }
    [WebMethod(EnableSession = true)]
    public static string GetPrevRandomImage()
    {
        return GetRandomImage(false);
    }
}
