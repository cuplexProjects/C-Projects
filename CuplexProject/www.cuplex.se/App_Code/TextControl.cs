using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using CuplexLib;

/// <summary>
/// Summary description for TextControl
/// </summary>
/// 
namespace CustomUserControls
{
    public class TextControl : Control
    {
        public string Text { get; set; }
        public string ResourceKey { get; set; }
        public string CssClass { get; set; }
        
        public TextControl()
        {

        }

        protected override void Render(HtmlTextWriter writer)
        {
            string controlText = "";
            if (!string.IsNullOrEmpty(ResourceKey))
            {
                controlText = Utils.GetResourceText(ResourceKey);
                if (controlText == null)
                    controlText = "#Missing Resource#";
            }
            else
                controlText = Text;

            if (!string.IsNullOrEmpty(CssClass))
            {
                writer.WriteBeginTag("span");
                writer.WriteAttribute("class", CssClass);
                writer.WriteEndTag("");
                writer.Write(controlText);
                writer.WriteEndTag("span");
            }
            else                
                writer.Write(controlText);
        }
    }
}