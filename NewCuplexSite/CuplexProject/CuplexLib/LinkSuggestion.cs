using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CuplexLib.LinqExtensions;
using CLinq = CuplexLib.Linq;

namespace CuplexLib
{
    public class LinkSuggestion
    {
        public int LinkSuggestionRef { get; private set; }
        public string LinkUrl { get; set; }
        public string Description { get; set; }
        public int CategoryRef { get; set; }
        public int UserRef { get; set; }
        public DateTime LinkSuggestionDate { get; set; }

        public LinkSuggestion()
        {
            this.LinkSuggestionRef = 0;
        }

        public void Save()
        {
            try
            {
                using (CLinq.DataContext db = CLinq.DataContext.Create())
                {
                    if (this.LinkSuggestionRef == 0)
                    {
                        CLinq.LinkSuggestion link = new CuplexLib.Linq.LinkSuggestion();
                        link.CategoryRef = this.CategoryRef;
                        link.Description = this.Description;
                        link.LinkUrl = this.LinkUrl;
                        link.UserRef = this.UserRef;
                        link.LinkSuggestionDate = this.LinkSuggestionDate;

                        db.LinkSuggestions.InsertOnSubmit(link);
                    }
                    else
                    {
                        CLinq.LinkSuggestion link = db.LinkSuggestions.Where(l => l.LinkSuggestionRef == this.LinkSuggestionRef).SingleOrDefault();
                        if (link == null) return;

                        link.CategoryRef = this.CategoryRef;
                        link.Description = this.Description;
                        link.LinkUrl = this.LinkUrl;
                        link.UserRef = this.UserRef;
                        link.LinkSuggestionDate = this.LinkSuggestionDate;
                    }
                    db.SubmitChanges();
                }
            }
            catch { }
        }

        public static LinkSuggestion GetOne(int linkSuggestionRef)
        {
            LinkSuggestion linkSuggestion = new LinkSuggestion();
            using (CLinq.DataContext db = CLinq.DataContext.Create())
            {
                CLinq.LinkSuggestion li = db.LinkSuggestions.Where(l => l.LinkSuggestionRef == linkSuggestionRef).SingleOrDefault();
                if (li == null) return null;

                linkSuggestion.LinkSuggestionRef = li.LinkSuggestionRef;
                linkSuggestion.CategoryRef = li.CategoryRef;
                linkSuggestion.Description = li.Description;
                linkSuggestion.LinkSuggestionDate = li.LinkSuggestionDate;
                linkSuggestion.LinkUrl = li.LinkUrl;
                linkSuggestion.UserRef = li.UserRef;
            }

            return linkSuggestion;
        }

        public static void DeleteMany(List<int> linkSuggestionRefList)
        {
            using (CLinq.DataContext db = CLinq.DataContext.Create())
            {
                db.LinkSuggestions.DeleteBatch(ls => linkSuggestionRefList.Contains(ls.LinkSuggestionRef));
            }
        }
        public static void CreateLinksFromLinkSuggestionList(List<int> linkSuggestionRefList, bool useLinkSuggestionDate)
        {
            using (CLinq.DataContext db = CLinq.DataContext.Create())
            {
                var linkSuggestionList = db.LinkSuggestions.Where(ls => linkSuggestionRefList.Contains(ls.LinkSuggestionRef)).ToList();

                foreach (var linkSuggestion in linkSuggestionList)
                {
                    CLinq.Link link = new CuplexLib.Linq.Link();
                    link.CategoryRef = linkSuggestion.CategoryRef;
                    if (useLinkSuggestionDate)
                        link.LinkDate = linkSuggestion.LinkSuggestionDate;
                    else
                        link.LinkDate = DateTime.Now;
                    link.LinkName = linkSuggestion.Description;
                    link.LinkUrl = linkSuggestion.LinkUrl;
                    link.UserRef = linkSuggestion.UserRef;

                    db.Links.InsertOnSubmit(link);
                }
                db.SubmitChanges();
            }
        }
    }
}