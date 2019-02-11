using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace CuplexLib
{
    public class DynamicLink : Control
    {
        public string LinkUrl { get; private set; }
        public string Description { get; private set; }

        public DynamicLink(string linkUrl, string description)
        {
            this.LinkUrl = linkUrl;
            this.Description = description;
        }
        public override string ToString()
        {
            return "<a target='_blank' href='" + LinkUrl + "'>" + Description + "</a>";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.WriteBeginTag("a");
            writer.WriteAttribute("target", "_blank");
            writer.WriteAttribute("href", LinkUrl);
            writer.WriteLineNoTabs(Description);
            writer.WriteEndTag("a");
        } 
    }
}