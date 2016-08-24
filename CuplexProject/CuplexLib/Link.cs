using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CuplexLib.LinqExtensions;
using CLinq = CuplexLib.Linq;

namespace CuplexLib
{
    public class Link
    {
        public int LinkRef { get; private set; }
        public string LinkName { get; set; }
        public DateTime LinkDate { get; set; }
        public int CategoryRef { get; set; }
        public int? UserRef { get; set; }
        public string UserName { get; private set; }
        public string LinkUrl { get; set; }
        public int Clicks { get; set; }
        public double Rating { get; set; }

        public static Link GetOne(int linkRef)
        {
            Link link = new Link();
            using (CLinq.DataContext db = CLinq.DataContext.Create())
            {
                CLinq.Link dbLink = db.Links.Where(l => l.LinkRef == linkRef).SingleOrDefault();
                if (dbLink == null) return null;

                link.LinkRef = dbLink.LinkRef;
                link.CategoryRef = dbLink.CategoryRef;
                link.Clicks = dbLink.Clicks;
                link.LinkDate = dbLink.LinkDate;
                link.LinkName = dbLink.LinkName;
                link.LinkUrl = dbLink.LinkUrl;
                link.Rating = dbLink.Rating;
                link.UserRef = dbLink.UserRef;
                if (link.UserRef != null)
                    link.UserName = dbLink.User.UserName;
            }
            return link;
        }

        public static void DeleteMany(List<int> linkRefList)
        {
            using (CLinq.DataContext db = CLinq.DataContext.Create())
            {
                db.UserToLinks.DeleteBatch(utl => linkRefList.Contains(utl.LinkRef));
                db.Comments.DeleteBatch(c => linkRefList.Contains(c.LinkRef));
                db.Links.DeleteBatch(l => linkRefList.Contains(l.LinkRef));
            }
        }

        public void Save()
        {
            using (CLinq.DataContext db = CLinq.DataContext.Create())
            {
                CLinq.Link link;
                if (this.LinkRef > 0)
                {
                    link = db.Links.Where(l => l.LinkRef == this.LinkRef).SingleOrDefault();
                    if (link == null) return;

                    link.CategoryRef = this.CategoryRef;
                    link.Clicks = this.Clicks;
                    link.LinkDate = this.LinkDate;
                    link.LinkName = this.LinkName;
                    link.LinkUrl = this.LinkUrl;
                    link.Rating = this.Rating;
                    link.UserRef = this.UserRef;
                }
                else
                {
                    link = new CuplexLib.Linq.Link();
                    link.CategoryRef = this.CategoryRef;
                    link.Clicks = this.Clicks;
                    link.LinkDate = this.LinkDate;
                    link.LinkName = this.LinkName;
                    link.LinkUrl = this.LinkUrl;
                    link.Rating = this.Rating;
                    link.UserRef = this.UserRef;

                    db.Links.InsertOnSubmit(link);
                }

                db.SubmitChanges();
            }
        }
    }
}
