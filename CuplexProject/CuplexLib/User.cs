using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CuplexLib
{
    public class User
    {
        public int UserRef { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; private set; }
        public DateTime? LastLogin { get; set; }

        public static User GetOne(int userRef)
        {
            User usr = null;
            using (CuplexLib.Linq.DataContext db = CuplexLib.Linq.DataContext.Create())
            {
                CuplexLib.Linq.User user = db.Users.Where(u => u.UserRef == userRef).SingleOrDefault();
                if (user != null)
                {
                    usr = new User();
                    usr.UserRef = user.UserRef;                    
                    usr.UserName = user.UserName;
                    usr.EmailAddress = user.EmailAddress;
                    usr.PasswordHash = user.PasswordHash;
                    usr.IsAdmin = user.IsAdmin;
                    usr.LastLogin = user.LastLogin;
                }
            }

            return usr;
        }
        public static User GetOneByUserName(string userName)
        {
            User usr = null;
            using (CuplexLib.Linq.DataContext db = CuplexLib.Linq.DataContext.Create())
            {
                CuplexLib.Linq.User user = db.Users.Where(u => u.UserName == userName).SingleOrDefault();
                if (user != null)
                {
                    usr = new User();
                    usr.UserRef = user.UserRef;
                    usr.UserName = user.UserName;
                    usr.EmailAddress = user.EmailAddress;
                    usr.IsAdmin = user.IsAdmin;
                    usr.LastLogin = user.LastLogin;
                }
            }

            return usr;
        }
        public static bool UserNameExists(string userName)
        {
            using (CuplexLib.Linq.DataContext db = CuplexLib.Linq.DataContext.Create())
            {
                return db.Users.Any(u => u.UserName == userName);
            }
        }
        public static User CreateUser(string userName, string password, string emailAddress)
        {
            User user = null;
            using (CuplexLib.Linq.DataContext db = CuplexLib.Linq.DataContext.Create())
            {
                if (db.Users.Any(u => u.UserName == userName))
                    return null;

                CuplexLib.Linq.User dbUser = new CuplexLib.Linq.User();
                dbUser.UserName = userName;
                dbUser.PasswordHash = Utils.GetMd5Hash(password);
                dbUser.EmailAddress = emailAddress;

                db.Users.InsertOnSubmit(dbUser);
                db.SubmitChanges();

                user = new User();
                user.EmailAddress = emailAddress;
                user.UserName = userName;
                user.UserRef = dbUser.UserRef;
                user.PasswordHash = dbUser.PasswordHash;
            }

            return user;
        }
        public static User UpdateUser(int userRef, string password, string emailAddress)
        {
            using (CuplexLib.Linq.DataContext db = CuplexLib.Linq.DataContext.Create())
            {
                CuplexLib.Linq.User user = db.Users.Where(u => u.UserRef == userRef).SingleOrDefault();
                if (user == null) return null;
                if (password != null)
                    user.PasswordHash = Utils.GetMd5Hash(password);
                user.EmailAddress = emailAddress;
                db.SubmitChanges();

                return GetOne(userRef);
            }
        }

        public static UserAuthenticateResponce AuthenticateUser(string userName, string password)
        {
            using (CuplexLib.Linq.DataContext db = CuplexLib.Linq.DataContext.Create())
            {
                var user = db.Users.Where(u => u.UserName == userName).SingleOrDefault();

                if (user == null)
                    return UserAuthenticateResponce.UnknownUser;

                if (user.PasswordHash == Utils.GetMd5Hash(password))
                    return UserAuthenticateResponce.PasswordCorrect;
                else
                    return UserAuthenticateResponce.PasswordIncorrect;
            }
        }

        public enum UserAuthenticateResponce
        {
            UnknownUser = 1,
            PasswordIncorrect = 2,
            PasswordCorrect = 3
        }

        public void save()
        {
            if (UserRef > 0)
            {
                using (CuplexLib.Linq.DataContext db = CuplexLib.Linq.DataContext.Create())
                {
                    CuplexLib.Linq.User dbUser = db.Users.Where(u => u.UserRef == UserRef).SingleOrDefault();
                    if (dbUser == null) return;

                    dbUser.LastLogin = LastLogin;
                    dbUser.EmailAddress = EmailAddress;
                    
                    db.SubmitChanges();
                }
            }
        }
    }
}
