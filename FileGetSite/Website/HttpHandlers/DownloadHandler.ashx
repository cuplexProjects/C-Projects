<%@ WebHandler Language="C#" Class="DownloadHandler" %>

using System;
using System.Web;
using System.Collections;

public class DownloadHandler : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        if (context.Session["IsAuthorized"] == null || context.Session["linkId"] == null)
        {
            context.Response.StatusCode = 401;
            response.Flush();
            response.End();
            return;
        }
        string linkId = context.Session["linkId"] as string;

        Hashtable htFiles = HttpRuntime.Cache.Get("AvailableFileHashTable_" + linkId) as Hashtable;
        if (htFiles == null)
        {
            context.Response.StatusCode = 401;
            response.Write("Link timeout, please refresh download link page");
            response.Flush();
            response.End();
            return;
        }

        string fileName = htFiles[context.Request.QueryString["id"]] as string;
        if (fileName == null)
        {
            context.Response.StatusCode = 401;
            response.Write("The file was not found on the server");
            response.Flush();
            response.End();
            return;
        }

        string mimeType = "text/plain";

        try
        {
            mimeType = MimeExtensionHelper.GetMimeType(fileName);
        }
        catch (Exception ex)
        {
            response.Write(ex.Message);
            response.Flush();
            response.End();
            return;            
        }
            
        
        response.ClearContent();
        response.Clear();        
        response.ContentType = mimeType;
        response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ";");
        response.TransmitFile(fileName);
        response.Flush();
        response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}