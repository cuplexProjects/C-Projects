using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Clinq = CuplexLib.Linq;
using System.Runtime.Serialization;

namespace CuplexLib
{
    [Serializable]
    public class Bookmark : ISerializable
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
        public Bookmark Parent { get; set; }
        public List<Bookmark> Children { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime DateAdded { get; set; }
        public BookmarkTypes BookmarkType { get; set; }

        public Bookmark()
        {
        }

        public Bookmark(SerializationInfo info, StreamingContext context)
        {
            this.Name = info.GetString("Name");
            this.Url = info.GetString("Url");
            this.SortOrder = info.GetInt32("SortOrder");
            this.LastModified = info.GetDateTime("LastModified");
            this.DateAdded = info.GetDateTime("DateAdded");
            this.BookmarkType = (BookmarkTypes)info.GetValue("BookmarkType", typeof(BookmarkTypes));

            if (info.GetValue("ChildCount", typeof(int)) != null)
            {
                int childCount = info.GetInt32("ChildCount");
                this.Children = new List<Bookmark>();
                for (int i = 0; i < childCount; i++)
                {
                    this.Children.Add(info.GetValue("Child" + i.ToString(), typeof(Bookmark)) as Bookmark);
                }
            }
            this.Parent = info.GetValue("Parent", typeof(Bookmark)) as Bookmark;
        }

        public static void SaveBookmarkListToDB(int userRef, BookmarkList bookmarkList)
        {
            try
            {
                CuplexLib.Encryption.Crypto crypto = new Encryption.Crypto();
                string bookmarkData = RuntimeSerialization.SerializeDataStructure(bookmarkList);
                bookmarkData = crypto.Encrypting(bookmarkData, userRef.ToString());

                using (var db = Clinq.DataContext.Create())
                {
                    Clinq.Bookmark bookMark = new Clinq.Bookmark();
                    bookMark.UserRef = userRef;
                    bookMark.BookmarkData = bookmarkData;
                    db.Bookmarks.InsertOnSubmit(bookMark);

                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                EventLog.SaveToEventLog(ex.Message, EventLogType.Error, userRef);
            }
        }

        public static BookmarkList ParseJsonBookmarkFile(int userRef, string fileName)
        {
            BookmarkList bookmarkList = new BookmarkList();
            try
            {
                FileStream fs = File.OpenRead(fileName);
                StreamReader sr = new StreamReader(fs);


                fs.Close();
                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                EventLog.SaveToEventLog(ex.Message, EventLogType.Error, userRef);
                return null;
            }
            return bookmarkList;
        }

        public enum BookmarkTypes
        {
            Link = 1,
            Folder = 2
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", this.Name);
            info.AddValue("Url", this.Url);
            info.AddValue("SortOrder", this.SortOrder);
            info.AddValue("LastModified", this.LastModified);
            info.AddValue("DateAdded", this.DateAdded);
            info.AddValue("BookmarkType", this.BookmarkType);

            if (this.Parent!=null)
                info.AddValue("Parent", this.Parent);

            if (this.Children != null)
            {
                info.AddValue("ChildCount", this.Children.Count);
                for (int i = 0; i < this.Children.Count;i++ )
                    info.AddValue("Child" + i.ToString(), this.Children[i]);
            }
        }
    }

    [Serializable]
    public class BookmarkList : List<Bookmark>, ISerializable
    {
        public BookmarkList()
        {
        }
        public BookmarkList(SerializationInfo info, StreamingContext context)
        {
            for (int i = 0; i < info.MemberCount;i++ )
            {
                Bookmark b = info.GetValue("BookmarkItem" + i.ToString(), typeof(Bookmark)) as Bookmark;
                if (b != null)
                    this.Add(b);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            for (int i = 0; i < this.Count; i++)
            {
                info.AddValue("BookmarkItem" + i.ToString(), this[i]);
            }
        }
    }
}