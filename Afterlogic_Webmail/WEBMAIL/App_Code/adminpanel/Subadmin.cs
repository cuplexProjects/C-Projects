using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;

namespace WebMail
{
    public class Subadmin
    {
        #region Fields
        protected int _id;
        protected string _login;
        protected string _password;
        protected string _description;
        #endregion

        #region Properties
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }
        public string Password
        {
            get { return AdminPanelUtils.DecryptPassword(_password); }
            set { _password = AdminPanelUtils.EncryptPassword(value); }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        #endregion

        public Subadmin()
        {
            _id = -1;
            _login = string.Empty;
            _password = string.Empty;
            _description = string.Empty;
        }

        public Subadmin(int id, string login, string password, string description)
        {
            _id = id;
            _login = login;
            _password = AdminPanelUtils.EncryptPassword(password);
            _description = description;
        }

        public static bool IsSubadminDomain(int id_admin, int id_domain)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            List<int> result = new List<int>();
            try
            {
                dbMan.Connect();
                result = dbMan.SelectSubadminDomains(id_admin);
            }
            finally
            {
                dbMan.Disconnect();
            }

            if (result.Contains(id_domain))
            {
                return true;
            }
            return false;
        }

        public static List<int> GetDomainsId(int id_admin)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            List<int> result = new List<int>();
            try
            {
                dbMan.Connect();
                result = dbMan.SelectSubadminDomains(id_admin);
            }
            finally
            {
                dbMan.Disconnect();
            }

            return result;
        }

        public static DomainCollection GetDomains(int id_admin)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            DomainCollection result = new DomainCollection();
            try
            {
                dbMan.Connect();
                result = dbMan.SelectDomainsByAdmin(id_admin);
            }
            finally
            {
                dbMan.Disconnect();
            }
            return result;
        }

        public static Subadmin GetSubadmin(int id_admin)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            Subadmin result = null;
            try
            {
                dbMan.Connect();
                result = dbMan.SelectSubadmin(id_admin);
            }
            finally
            {
                dbMan.Disconnect();
            }
            return result;
        }

        public static Subadmin GetSubadmin(string admin)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            Subadmin result = null;
            try
            {
                dbMan.Connect();
                result = dbMan.SelectSubadmin(admin);
            }
            finally
            {
                dbMan.Disconnect();
            }
            return result;
        }

        public static SubadminCollection GetSubadmins(int page, int pageSize, string orderBy, bool asc, string searchCondition)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            SubadminCollection result = null;
            try
            {
                dbMan.Connect();
                result = dbMan.SelectSubadmins(page, pageSize, orderBy, asc, searchCondition);
            }
            finally
            {
                dbMan.Disconnect();
            }
            return result;
        }


        public static void Update(int id_admin, string login, string password, string description, List<string> domains)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            try
            {
                dbMan.Connect();
                dbMan.UpdateSubadmin(id_admin, login, password, description, domains);
            }
            finally
            {
                dbMan.Disconnect();
            }
        }

        public static int Create(string login, string password, string description, List<string> domains)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            int result = 0;
            try
            {
                dbMan.Connect();
                result = dbMan.CreateSubadmin(login, password, description, domains);
            }
            finally
            {
                dbMan.Disconnect();
            }
            return result;
        }

        public static int Count(string condition)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            int result = 0;
            try
            {
                dbMan.Connect();
                result = dbMan.GetSubadminCount(condition);
            }
            finally
            {
                dbMan.Disconnect();
            }
            return result;
        }

        public static void Delete(int id_admin)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            try
            {
                dbMan.Connect();
                dbMan.DeleteSubadmin(id_admin);
            }
            finally
            {
                dbMan.Disconnect();
            }
        }
    }

    public class SubadminCollection : CollectionBase
    {
        public SubadminCollection()
        {
        }

        public Subadmin this[int index]
        {
            get { return ((Subadmin)List[index]); }
            set { List[index] = value; }
        }

        public Subadmin GetItem(int ID)
        {
            foreach (Subadmin adm in List)
            {
                if (adm.ID == ID)
                {
                    return adm;
                }
            }
            return null;
        }

        public int Add(Subadmin value)
        {
            return (List.Add(value));
        }

        public int IndexOf(Subadmin value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, Subadmin value)
        {
            List.Insert(index, value);
        }

        public void Remove(Subadmin value)
        {
            List.Remove(value);
        }

        public bool Contains(Subadmin value)
        {
            return (List.Contains(value));
        }
    }
}
