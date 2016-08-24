using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.Adapters;
using System.Web.UI;
using System.IO;

/// <summary>
/// Summary description for FormRewriterControlAdapter
/// </summary>
public class FormRewriterControlAdapter : ControlAdapter
{
    public FormRewriterControlAdapter()
    {

    }
    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        base.Render(new RewriteFormHtmlTextWriter(writer));
    }
}

public class RewriteFormHtmlTextWriter : HtmlTextWriter
{
    public RewriteFormHtmlTextWriter(HtmlTextWriter writer)
        : base(writer)
    {
        
        base.InnerWriter = writer.InnerWriter;
    }
    public RewriteFormHtmlTextWriter(TextWriter writer)
        : base(writer)
    {        
        base.InnerWriter = writer;
    }
    public override void WriteAttribute(string name, string value, bool fEncode)
    {
        if (name == "action")
        {
            var contect = HttpContext.Current;
            if (contect.Items["ActionAlreadyWritten"] == null)
            {
                value = contect.Request.RawUrl;
                contect.Items["ActionAlreadyWritten"] = true;
            }

        }
        base.WriteAttribute(name, value, fEncode);
    }
}
