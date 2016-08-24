using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;

namespace WebMail
{
    public class HMailServer
    {
        protected System.Reflection.Assembly _assembly = null;
        protected object _application = null;
        protected bool _connected = false;

        public System.Reflection.Assembly assembly
        {
            get { return _assembly; }
        }

        public bool Connected
        {
            get { return _connected; }
        }

        public object application
        {
            get { return _application; }
        }

        public bool IsLoaded
        {
            get 
            {
                if (_assembly != null)
                {
                    return true;
                }
                return false; ; 
            }
        }

        public HMailServer()
        {
            string AssemblyPath = Path.Combine(AdminPanelUtils.GetWebMailFolder(), @"bin\Interop.hMailServer.dll");
            if (File.Exists(AssemblyPath))
            {
                try
                {
                    _assembly = System.Reflection.Assembly.LoadFrom(AssemblyPath);
                    _application = assembly.CreateInstance("hMailServer.ApplicationClass");
                }
                catch (Exception ex)
                {
                    _assembly = null;
                    Log.WriteException(ex);
                    throw;
                }
            }
        }

        public bool Connect()
        {
            if (Connected == true) return Connected;
            if (assembly != null && application != null)
            {
                Type appType = application.GetType();

                string dataFolder = Utils.GetDataFolderPath();
                string filePath = Path.Combine(dataFolder, @"settings\hmailserver.txt");

                if (File.Exists(filePath))
                {
                    string srvadm = string.Empty;
                    string srvpass = string.Empty;

                    string[] lines = File.ReadAllLines(filePath);

                    if (lines.Length > 1)
                    {
                        srvadm = lines[0];
                        srvpass = lines[1];
                    }

                    System.Reflection.MethodInfo methodAuthenticate = appType.GetMethod("Authenticate");
                    object admin = methodAuthenticate.Invoke(application, new object[] { srvadm, srvpass });

                    if (admin != null)
                    {
                        _connected = true;
                        return true;
                    }
                }
                else
                {
                    return false;
                }
                return false;
            }
            return false;
        }

        public object GetAccount(string domainName, string email)
        {
            if (Connect())
            {
                Type appType = application.GetType();

                object domains = appType.GetProperty("Domains").GetValue(application, null);
                Type domainsType = domains.GetType();

                System.Reflection.MethodInfo methodget_ItemByName = domainsType.GetMethod("get_ItemByName");
                object domain = methodget_ItemByName.Invoke(domains, new object[] { domainName });

                if (domain != null)
                {
                    Type domainType = domain.GetType();

                    object accounts = domainType.GetProperty("Accounts").GetValue(domain, null);
                    Type accountsType = accounts.GetType();

                    System.Reflection.MethodInfo methodget_ItemByAddress = accountsType.GetMethod("get_ItemByAddress");
                    return methodget_ItemByAddress.Invoke(accounts, new object[] { email });
                }
            }
            return null;
        }

        public bool UpdateUserPassword(string domainName, string email, string password)
        {
            if (Connect())
            {
                object account = GetAccount(domainName, email);

                if (account != null)
                {
                    Type accountType = account.GetType();
                    
                    System.Reflection.PropertyInfo propertyPassword = accountType.GetProperty("Password");
                    propertyPassword.SetValue(account, password, null);
                    
                    System.Reflection.MethodInfo methodSave = accountType.GetMethod("Save");
                    methodSave.Invoke(account, null);
                    return true;
                }
            }
            return false;
        }

        public bool UpdateAutoResponder(string domainName, string email, bool Enable, string Subject, string Message)
        {
            if (Connect())
            {
                object account = GetAccount(domainName, email);

                if (account != null)
                {
                    Type accountType = account.GetType();

                    System.Reflection.PropertyInfo propertyMessage = accountType.GetProperty("VacationMessage");
                    propertyMessage.SetValue(account, Message, null);

                    System.Reflection.PropertyInfo propertySubject = accountType.GetProperty("VacationSubject");
                    propertySubject.SetValue(account, Subject, null);

                    System.Reflection.PropertyInfo propertyMessageIsOn = accountType.GetProperty("VacationMessageIsOn");
                    propertyMessageIsOn.SetValue(account, Enable, null);

                    System.Reflection.MethodInfo methodSave = accountType.GetMethod("Save");
                    methodSave.Invoke(account, null);
                    return true;
                }
            }
            return false;
        }

        public Autoresponder GetAutoResponder(string domainName, string email)
        {
            Autoresponder aresponder = null;
            if (Connect())
            {
                object account = GetAccount(domainName, email);

                if (account != null)
                {
                    Type accountType = account.GetType();

                    aresponder = new Autoresponder();
                    aresponder.Message = Convert.ToString(accountType.GetProperty("VacationMessage").GetValue(account, null));
                    aresponder.Subject = Convert.ToString(accountType.GetProperty("VacationSubject").GetValue(account, null));
                    aresponder.Enable = Convert.ToBoolean(accountType.GetProperty("VacationMessageIsOn").GetValue(account, null));
                }
            }
            return aresponder;
        }
    }
}
