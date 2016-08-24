using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using CuplexLib;

/// <summary>
/// Summary description for WebUtils
/// </summary>
public class WebUtils
{
    private WebUtils()
    {
       
    }
    public static List<RandomImage> GetRandomImageListFromCache()
    {
        List<RandomImage> randomImageList = HttpRuntime.Cache["randomImageList"] as List<RandomImage>;
        if (randomImageList == null)
        {
            randomImageList = RandomImage.GetRandomImageList();
            HttpRuntime.Cache.Insert("randomImageList", randomImageList, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        return randomImageList;
    }
    public static string[] GetRandomImgFileList()
    {
        string path = HttpContext.Current.Request.PhysicalApplicationPath + "RandomImg\\";
        string[] fileList = Directory.GetFiles(path);


        for (int i = 0; i < fileList.Length; i++)
        {
            string fileName = fileList[i];
            int iPos = fileName.LastIndexOf('\\');
            if (iPos > 0)
            {
                fileName = fileName.Substring(iPos + 1);
                fileList[i] = fileName;
            }
        }

        return fileList;
    }
}
