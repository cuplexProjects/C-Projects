using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DownloadHttpHandler
/// </summary>
public class DownloadHttpHandler : IHttpHandler
{
	public DownloadHttpHandler()
	{

	}

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Redirect("~/HttpHandlers/DownloadHandler.ashx?id=" + context.Request.Url.Segments.Last());
    }
}