using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLinq = CuplexLib.Linq;

namespace CuplexLib
{
    public class Comment
    {
        public int CommentRef { get; private set; }
        public DateTime CommentDate { get; set; }
        public int LinkRef { get; set; }
        public string CommentText { get; set; }        
        public int UserRef { get; set; }
        public string UserName { get; private set; }

        public Comment()
        {
            this.CommentRef = 0;
        }

        public void Save()
        {
            try
            {
                using (CLinq.DataContext db = CLinq.DataContext.Create())
                {
                    if (this.CommentRef == 0)
                    {
                        CLinq.Comment comment = new CuplexLib.Linq.Comment();
                        comment.CommentDate = this.CommentDate;
                        comment.CommentText = this.CommentText;
                        comment.LinkRef = this.LinkRef;
                        comment.UserRef = this.UserRef;

                        db.Comments.InsertOnSubmit(comment);
                    }
                    else
                    {
                        CLinq.Comment comment = db.Comments.Where(c => c.CommentRef == this.CommentRef).SingleOrDefault();
                        if (comment == null) return;

                        comment.CommentDate = this.CommentDate;
                        comment.CommentText = this.CommentText;
                        comment.LinkRef = this.LinkRef;
                        comment.UserRef = this.UserRef;
                    }
                    db.SubmitChanges();
                }
            }
            catch { }
        }
        public static List<Comment> GetCommentListForLink(int linkRef)
        {
            using (CLinq.DataContext db = CLinq.DataContext.Create())
            {
                var commentQuery =
                from c in db.Comments
                where c.LinkRef == linkRef
                orderby c.CommentDate
                select new Comment
                {
                    CommentRef = c.CommentRef,
                    CommentDate = c.CommentDate,
                    CommentText = c.CommentText,
                    LinkRef = c.LinkRef,
                    UserRef = c.UserRef,
                    UserName = c.User.UserName
                };

                return commentQuery.ToList();
            }
        }
    }
}
