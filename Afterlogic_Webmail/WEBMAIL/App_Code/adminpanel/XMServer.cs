using System;
using System.Collections.Generic;
using XMailAdminProxy;
using System.Text.RegularExpressions;
using System.IO;
using WebMail;
using System.Collections;
using System.Web;
using System.Text;

public class XMUser
{
    [Serializable]
    public struct RedirectionsList
    {
        public List<string> forwards;
        public List<string> activeForwards;
    }

    [Serializable]
    public struct ExternalLinkObject
    {
        public string server;
        public string user;
    }

    [Serializable]
    public struct UserInfo
    {
        public string domain;
        public string user;
        public string pass;
        public string type;
    }
    
    public static void ChangePassword(string domainName, string userName, string userPassword)
    {
        try
        {
            string res = string.Empty;
            if (userPassword != string.Empty)
            {
                XMLayer xmLayer = new XMLayer();
                xmLayer = XMServer.CreateXMLayer();
                xmLayer.Login();
                res = xmLayer.ChangePassword(domainName, userName, userPassword);
                xmLayer.Logout();
                if (res == "+00000 OK") res = string.Empty;
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
    }

    public static void DeleteUser(string domainName, string userName)
    {
        try
        {
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            xm.Login();
            xm.DeleteUser(domainName, userName);
            xm.Logout();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;            
        }
    }

    public static void AddListUser(string domainName, string userName, string NewUserAddressID, MLPERMISSIONS permisson)
    {
        try
        {
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            xm.Login();
            xm.AddMailingListUser(domainName, userName, NewUserAddressID, permisson);
            xm.Logout();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
    }

    public static void DeleteListUser(string domainName, string userName, string address)
    {
        try
        {
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            xm.Login();
            xm.DeleteMailingListUser(domainName, userName, address.Trim());
            xm.Logout();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
    }

    public static void AddNewAlias(string domainName, string userName, string AccountAliasID)
    {
        try
        {
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            xm.Login();
            xm.AddAlias(domainName, userName, AccountAliasID);
            xm.Logout();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
    }

    public static void AddUser(string domain, string login, string password, bool MailingList, double MaxMailboxSize, bool StoreMailLocaly)
    {
        try
        {
            List<string> varNames = new List<string>();
            List<string> varValues = new List<string>();

            varNames.Add("MaxMBSize");
            varValues.Add(MaxMailboxSize.ToString().Trim());

            varNames.Add("StoreMailLocaly");
            if (StoreMailLocaly) varValues.Add("1");
            else varValues.Add("0");

            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            xm.Login();
            xm.AddUser(domain, login, password, (MailingList == true) ? "M" : "U");
            xm.SetUserVariable(domain, login, varNames.ToArray(), varValues.ToArray());
            xm.Logout();

        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
    }

    public static void DeleteAlias(string domainName, string aliasNameForDelete)
    {
        XMLayer xm = new XMLayer();
        try
        {
            xm = XMServer.CreateXMLayer();
            xm.Login();
            xm.DeleteAlias(domainName, aliasNameForDelete);
            xm.Logout();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
    }

    public static void SaveUserVariables(string domainName, string userName, string MaxMailboxSize, bool StoreMailLocaly)
    {
        try
        {
            List<string> varNames = new List<string>();
            List<string> varValues = new List<string>();

            varNames.Add("MaxMBSize");
            varValues.Add(MaxMailboxSize.Trim());

            varNames.Add("StoreMailLocaly");
            if (StoreMailLocaly) varValues.Add("1");
            else varValues.Add("0");

            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            xm.Login();
            xm.SetUserVariable(domainName, userName, varNames.ToArray(), varValues.ToArray());
            xm.Logout();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
    }

    public static void AddExternalLink(string domainName, string userName, string ExternalPOP3Server, string ExternalUserName, string ExternalPassword, AUTHTYPE authType)
    {
        try
        {
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            xm.Login();
            xm.AddPop3ExternalLink(domainName, userName,ExternalPOP3Server,
                                                        ExternalUserName,
                                                        ExternalPassword, authType);
            xm.Logout();

        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
    }

    public static void DeleteExternalLink(string domainName, string userName, string extDomainName, string extUserName)
    {
        try
        {
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            xm.Login();
            xm.DeletePop3ExternalLink(domainName, userName, extDomainName.Trim(), extUserName.Trim());
            xm.Logout();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static void AdvancedSaveChanges(string domainName, string userName, string Advanced)
    {
        try
        {
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            xm.Login();
            string data = XMUser.GetMailProcTab(domainName, userName)[0];
            data += Advanced.Trim();
            data += "\r\n.\r\n";
            xm.SetMailProcTab(domainName, userName, data);
            xm.Logout();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
    }


    public static bool UserExist(string dName, string uName)
    {
        bool res = false;
        string line = string.Empty;
        string users = string.Empty;
        Regex re = new Regex("\\s+");

        try
        {
            XMLayer xmLayer = new XMLayer();
            xmLayer = XMServer.CreateXMLayer();
            xmLayer.Login();
            users = xmLayer.ListUsers(dName, uName);
            xmLayer.Logout();

            if (users.Contains(dName) && users.Contains(uName)) res = true;
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return res;
    }
    
    public static UserInfo GetUser(string dName, string uName)
    {
        UserInfo user = new UserInfo();
        string line = string.Empty;
        string users = string.Empty;
        Regex re = new Regex("\\s+");

        try
        {
            XMLayer xmLayer = new XMLayer();
            xmLayer = XMServer.CreateXMLayer();
            xmLayer.Login();
            users = xmLayer.ListUsers(dName, uName);
            xmLayer.Logout();

            using (StringReader sr = new StringReader(users))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == ".") break;
                    string[] us = re.Split(line);
                    user.domain = us[0].Trim(new char[] { '"' });
                    user.user = us[1].Trim(new char[] { '"' });
                    user.pass = us[2].Trim(new char[] { '"' });
                    user.type = us[3].Trim(new char[] { '"' });
                }
            }

        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return user;
    }

    public static List<string> GetListMembers(string dName, string uName)
    {
        List<string> userList = new List<string>();
        try
        {
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            xm.Login();
            string raw = xm.ListMailingListUsers(dName, uName);
            xm.Logout();
            string line;
            using (StringReader sr = new StringReader(raw))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == ".") break;
                	string userAddress = extractMailingListUserInfo(line, "Address");
                	string userAccess = "";
                	string userAccessType = extractMailingListUserInfo(line, "AccessType");
					if (userAccessType.ToUpper().IndexOf("R") > -1)
					{
						userAccess = "Read";
					}
					if ((userAccessType.ToUpper().IndexOf("R") > -1) && 
						(userAccessType.ToUpper().IndexOf("W") > -1 || 
						userAccessType.ToUpper().IndexOf("A") > -1))
					{
						userAccess = "Read/Post";
					}
					userList.Add(userAddress + " (" + userAccess + ")");
                }
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return userList;
    }

    public static string extractMailingListUserInfo(string complexUserName, string infoType)
    {
        try
        {
            Regex re = new Regex("\\s+");
            string[] parts = re.Split(complexUserName);

            if (infoType == "Address") return parts[0].Trim(new char[] { '"' });
            if (infoType == "AccessType") return parts[1].Trim(new char[] { '"' });
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return "undefined";
    }

    public static List<string> GetUsersAliases(string dName, string uName)
    {
        List<string> userAliases = new List<string>();
        try
        {
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            xm.Login();
            string raw = "";
            string line;
            raw = xm.ListAliases(dName, "*", uName);
            using (StringReader sr = new StringReader(raw))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == ".") break;
                    userAliases.Add(extractUsersAliasName(line));
                }
            }
            xm.Logout();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return (userAliases.Count == 0) ? null : userAliases;
    }

    public static string extractUsersAliasName(string line)
    {
        try
        {
            Regex re = new Regex("\\s+");
            string[] parts = re.Split(line);
            return parts[1].Trim(new char[] { '"' });
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return "";
    }

    public static string[] GetUserVariables(string dName, string uName)
    {
        string[] userVariables = new string[5];
        for (int i = 0; i < userVariables.Length; i++) userVariables[i] = " ";
        try
        {
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            string raw = "";
            string line;
            xm.Login();
            raw = xm.ListUserVariables(dName, uName);
            xm.Logout();

            using (StringReader sr = new StringReader(raw))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == ".") break;
                    switch (extractUsersVariable(line, false))
                    {
                        case "RealName":
                            userVariables[0] = extractUsersVariable(line, true);
                            break;
                        case "HomePage":
                            userVariables[1] = extractUsersVariable(line, true);
                            break;
                        case "MaxMBSize":
                            userVariables[2] = extractUsersVariable(line, true);
                            break;
                        case "StoreMailLocaly":
                            userVariables[3] = extractUsersVariable(line, true);
                            break;
                        case "ClosedMailList":
                            userVariables[4] = extractUsersVariable(line, true);
                            break;
                        default:

                            break;
                    }
                }
                if (userVariables[3] == " ") userVariables[3] = "1";
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return userVariables;
    }

    public static string extractUsersVariable(string line, bool flag)
    {
        try
        {
            Regex re = new Regex("\\s+");
            string[] parts = re.Split(line);
            if (flag) return parts[1].Trim(new char[] { '"' });
            else return parts[0].Trim(new char[] { '"' });
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return "";
    }
    public static RedirectionsList GetRedirections(string dName, string uName)//
    {
        RedirectionsList result = new RedirectionsList();
        result.forwards = new List<string>();
        result.activeForwards = new List<string>();
        try
        {
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            string raw = "";
            string line;
            xm.Login();
            raw = xm.GetMailProcTab(dName, uName);
            xm.Logout();
            using (StringReader sr = new StringReader(raw))
            {
                string[] data;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == ".") break;
                    data = extractMailProc(line);
                    switch (data[0])
                    {
                        case "redirect":
                            for (i = 1; i < data.Length; i++)
                                result.forwards.Add(data[i]);
                            break;
                        case "lredirect":
                            for (i = 1; i < data.Length; i++)
                                result.activeForwards.Add(data[i]);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return result;
    }

    public static string[] extractMailProc(string line)
    {
        string[] parts = null;
        try
        {
            Regex re = new Regex("\\s+");
            parts = re.Split(line);
            for (int i = 0; i < parts.Length; i++) parts[i] = parts[i].Trim(new char[] { '"' });
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return parts;
    }

    public static bool GetMailBoxInstruct(string dName, string uName)
    {
        try
        {
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            string raw = "";
            string line;
            xm.Login();
            raw = xm.GetMailProcTab(dName, uName);
            xm.Logout();
            using (StringReader sr = new StringReader(raw))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("mailbox")) return true;
                }
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return false;
    }

    public static void SetMailBoxInstruct(string dName, string uName)
    {
        try
        {
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            string raw = "";
            xm.Login();
            try
            {
                raw = xm.GetMailProcTab(dName, uName);
            }
            catch (XMailAdminProxy.XMailException) 
            { }
            raw = "\"mailbox\"\r\n" + raw;
            xm.SetMailProcTab(dName, uName, raw);
            xm.Logout();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
    }

    public static void DelMailBoxInstruct(string dName, string uName)
    {
        try
        {
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            string raw = string.Empty;
            string line = string.Empty;
            string res_lines = string.Empty;
            xm.Login();
            raw = xm.GetMailProcTab(dName, uName);
            StringWriter sw = new StringWriter();
            using (StringReader sr = new StringReader(raw))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (!line.Contains("mailbox"))
                    {
                       sw.WriteLine(line);
                    }
                }
            }
            xm.SetMailProcTab(dName, uName, sw.ToString());
            xm.Logout();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
    }

    public static List<ExternalLinkObject> GetExternalLink(string dName, string uName)
    {
        List<ExternalLinkObject> ExternalLink = new List<ExternalLinkObject>();
        XMLayer xm = new XMLayer();
        try
        {
            xm = XMServer.CreateXMLayer();
            xm.Login();
            string raw = "";
            string line;
            raw = xm.ListPop3ExternalLink(dName, uName);
            xm.Logout();
            using (StringReader sr = new StringReader(raw))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == ".") break;
                    ExternalLinkObject elobj = new ExternalLinkObject();
                    elobj.server = extractExternalLink(line, "SERVER_NAME");
                    elobj.user = extractExternalLink(line, "USER_NAME");
                    ExternalLink.Add(elobj);
                }
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return ExternalLink;
    }

    public static string extractExternalLink(string line, string param)
    {
        try
        {
            Regex re = new Regex("\\s+");
            string[] parts = re.Split(line);
            switch (param)
            {
                case "SERVER_NAME":
                    return parts[2].Trim(new char[] { '"' });
                case "USER_NAME":
                    return parts[3].Trim(new char[] { '"' });
                default:
                    break;
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return "error";
    }

    public static string getName(string line, string param)
    {
        try
        {
            Regex re = new Regex("\\s+");
            string[] parts = re.Split(line);
            switch (param)
            {
                case "SERVER":
                    return parts[1].Trim(new char[] { '"' });
                case "USER":
                    return parts[0].Trim(new char[] { '"' });
                default:
                    break;
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return "error";
    }

    public static string GetAdvanced(string dName, string uName)
    {
        string content = "";
        XMLayer xm = new XMLayer();
        try
        {
            xm = XMServer.CreateXMLayer();
            xm.Login();
            string raw = "";
            string line;

            raw = xm.GetMailProcTab(dName, uName);
            xm.Logout();
            using (StringReader sr = new StringReader(raw))
            {
                string[] data;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == ".") break;
                    data = extractMailProc(line);
                    switch (data[0])
                    {
                        case "mailbox":
                        case "redirect":
                        case "lredirect":
                            break;
                        default:
                            content += line + "\r\n";
                            break;
                    }
                }
            }
        }
        catch (XMailException error)
        {
            Log.WriteException(error);
        }
        return content;
    }

    public static string[] GetMailProcTab(string domainName, string userName)
    {
        string[] result = new string[2] { "", "" };
        XMLayer xm = new XMLayer();
        try
        {
            xm = XMServer.CreateXMLayer();
            xm.Login();
            string raw = xm.GetMailProcTab(domainName, userName);
            xm.Logout();

            string line;
            result[0] = "";
            result[1] = "";
            string[] id;
            using (StringReader sr = new StringReader(raw))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == ".") break;
                    id = extractMailProc(line);
                    line = line.Trim();
                    switch (id[0])
                    {
                        case "mailbox":
                        case "redirect":
                        case "lredirect":
                            result[0] += line + "\r\n";
                            break;
                        default:
                            result[1] += line + "\r\n";
                            break;
                    }
                }
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
        return result;
    }

    public static void AddDeleteRedirections(string domainName, string userName, string command, string element)
    {
        try
        {
            string data = "";
            string[] mailProcTabData;
            mailProcTabData = XMUser.GetMailProcTab(domainName, userName);

            string line;
            string head = "";

            string redirection = "";
            string lredirection = "";

            bool noRedirection = true;
            bool noLRedirection = true;

            string[] id;
            int lineIndex;
            StringReader reader = new StringReader(mailProcTabData[0]);
            switch (command)
            {
                case "AddToRedirection":
                case "AddToLRedirection":
                    lineIndex = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineIndex++;
                        id = XMUser.extractMailProc(line);
                        line = line.Trim();
                        switch (id[0])
                        {
                            case "mailbox":
                                head = line + "\r\n";
                                break;
                            case "redirect":
                                noRedirection = false;
                                if (command == "AddToRedirection")
                                    redirection = line + '\t' + '\"' + element + '\"' + "\r\n";
                                else
                                    redirection = line + "\r\n";
                                break;
                            case "lredirect":
                                noLRedirection = false;
                                if (command == "AddToLRedirection")
                                    lredirection = line + '\t' + '\"' + element + '\"' + "\r\n";
                                else
                                    lredirection = line + "\r\n";
                                break;
                            default:
                                break;
                        }
                    }
/*                    if (head == "")
                        head = '\"' + "mailbox" + '\"' + "\r\n";
*/ 
                    if (noRedirection)
                        if (command == "AddToRedirection") redirection = "redirect" + '\t' + '\"' + element + '\"' + "\r\n";
                    if (noLRedirection)
                        if (command == "AddToLRedirection") lredirection = "lredirect" + '\t' + '\"' + element + '\"' + "\r\n";
                    break;
                case "DeleteRedirection":
                case "DeleteLRedirection":
                    lineIndex = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineIndex++;
                        id = XMUser.extractMailProc(line);
                        line = line.Trim();
                        switch (id[0])
                        {
                            case "mailbox":
                                head = line + "\r\n";
                                break;
                            case "redirect":
                                if (command == "DeleteRedirection")
                                {
                                    int index = line.IndexOf(element);
                                    string before = line.Substring(0, index - 2);
                                    string after = line.Substring(index + 1 + element.Length);
                                    redirection = before + after + "\r\n";
                                    if (redirection.Length == 10) redirection = "";
                                }
                                else
                                {
                                    redirection = line + "\r\n";
                                }
                                break;
                            case "lredirect":
                                if (command == "DeleteLRedirection")
                                {
                                    int index = line.IndexOf(element);
                                    string before = line.Substring(0, index - 2);
                                    string after = line.Substring(index + 1 + element.Length);
                                    lredirection = before + after + "\r\n";
                                    if (lredirection.Length == 11) lredirection = "";
                                }
                                else
                                {
                                    lredirection = line + "\r\n";
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
            data = head + redirection + lredirection + mailProcTabData[1];
            XMLayer xm = new XMLayer();
            xm = XMServer.CreateXMLayer();
            xm.Login();
            xm.SetMailProcTab(domainName, userName, data);
            xm.Logout();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
    }
}


public class XMDomain
{
    public string Id;
    public string Name;
    public string Type;

    public XMDomain(string id, string name, string type)
    {
        Id = id;
        Name = name;
        Type = type;
    }

    public void Add()
    {
        XMLayer xmLayer = XMServer.CreateXMLayer();
        try
        {
            xmLayer.Login();
            xmLayer.AddDomain(Name);
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
        finally
        {
            xmLayer.Logout();
        }
    }

    public static void AddDomain(string domain)
    {
        XMLayer xmLayer = XMServer.CreateXMLayer();
        try
        {
            xmLayer.Login();
            xmLayer.AddDomain(domain);
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
        finally
        {
            xmLayer.Logout();
        }
    }

    public static XMDomainCollection GetDomains()
    {
        XMDomainCollection xmDomains = new XMDomainCollection();
        XMLayer xmLayer = new XMLayer();
        try
        {
            xmLayer = XMServer.CreateXMLayer();
            xmLayer.Login();

            string domains = xmLayer.ListDomains();
            using (StringReader sr = new StringReader(domains))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == ".") break;
                    string domainName = line.Trim(new char[] { '"' });
                    xmDomains.Add(new XMDomain(domainName, domainName, AdminPanelConstants.UserType.xm));
                }
            }
            xmLayer.Logout();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
        return xmDomains;
    }

    public static XMDomainCollection GetCustomDomainins()
    {
        XMDomainCollection xmDomains = new XMDomainCollection();
        XMLayer xmLayer = new XMLayer();
        try
        {
            xmLayer = XMServer.CreateXMLayer();
            xmLayer.Login();

            string domains = xmLayer.ListCustomDomains();
            using (StringReader sr = new StringReader(domains))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == ".") break;
                    string domainName = line.Trim(new char[] { '"' });
                    xmDomains.Add(new XMDomain(domainName, domainName, AdminPanelConstants.UserType.xm));
                }
            }
            xmLayer.Logout();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
        return xmDomains;
    }
    
    public static string[] GetCustomDomainTab(string customDomainName)
    {
        string[] result = new string[2];
        XMLayer xm = XMServer.CreateXMLayer();
        Hashtable ht = new Hashtable();

        try
        {
            xm.Login();
            string raw = xm.GetCustomDomainFile(customDomainName);
            string line;
            result[0] = "";
            result[1] = "";
            string[] id;

            using (StringReader sr = new StringReader(raw))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == ".") break;
                    id = extractCustomDomainData(line);
                    line = line.Trim();
                    switch (id[0])
                    {
                        case "mailbox":
                        case "redirect":
                        case "lredirect":
                            result[0] += line + "\r\n";
                            break;
                        default:
                            result[1] += line + "\r\n";
                            break;
                    }
                }
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
        finally
        {
            xm.Logout();
        }
        return result;
    }

    public static string[] extractCustomDomainData(string line)
    {
        string[] parts = null;
        try
        {
            Regex re = new Regex("\\s+");
            parts = re.Split(line);
            for (int i = 0; i < parts.Length; i++) parts[i] = parts[i].Trim(new char[] { '"' });
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
        return parts;
    }

    public static void DeleteCustomDomain(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            XMLayer xmLayer = new XMLayer();
            try
            {
                xmLayer = XMServer.CreateXMLayer();
                xmLayer.Login();
                xmLayer.DelCustomDomain(name);
                xmLayer.Logout();
            }
            catch (XMailException ex)
            {
                Log.WriteException(ex);
                throw;
            }
        }
    }

    public static void DeleteDomain(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            XMLayer xmLayer = new XMLayer();
            try
            {
                xmLayer = XMServer.CreateXMLayer();
                xmLayer.Login();
                xmLayer.DeleteDomain(name);
                xmLayer.Logout();
            }
            catch (XMailException ex)
            {
                Log.WriteException(ex);
                throw;
            }
        }
    }

}

public class XMServer
{
    public static XMailAdminProxy.XMLayer CreateXMLayer()
    {
        AdminPanelSettings apSettings = new AdminPanelSettings().Load();
        return CreateXMLayer(apSettings.User, apSettings.Pass, apSettings.Host, apSettings.Port);
    }

    public static XMailAdminProxy.XMLayer CreateXMLayer(string Login, string Password, string Host, string Port)
    {
        return new XMLayer(Login, Password, Host, Convert.ToInt32(Port));
    }
}

public class XMDomainCollection : CollectionBase
{
    public XMDomainCollection()
    {
    }

    public XMDomain this[int index]
    {
        get { return ((XMDomain)List[index]); }
        set { List[index] = value; }
    }

    public XMDomain GetItemID(string ID)
    {
        foreach (XMDomain dom in List)
        {
            if (dom.Id == ID)
            {
                return dom;
            }
        }
        return null;
    }

    public XMDomain GetItem(string Name)
    {
        foreach (XMDomain dom in List)
        {
            if (dom.Name == Name)
            {
                return dom;
            }
        }
        return null;
    }

    public int Add(XMDomain value)
    {
        return (List.Add(value));
    }

    public void Add(XMDomainCollection value)
    {
        foreach (XMDomain dom in value)
        {
            List.Add(value);
        }
    }

    public int IndexOf(XMDomain value)
    {
        return (List.IndexOf(value));
    }

    public void Insert(int index, XMDomain value)
    {
        List.Insert(index, value);
    }

    public void Remove(XMDomain value)
    {
        List.Remove(value);
    }

    public bool Contains(XMDomain value)
    {
        return (List.Contains(value));
    }
}

public class XMSettingsRow : IEquatable<XMSettingsRow>
{
    protected bool _isComment = false;
    protected string _key = null;
    protected string _value = null;

    public bool IsComment
    {
        get { return _isComment; }
        set { _isComment = value; }
    }

    public string Key
    {
        get { return _key; }
        set { _key = value; }
    }

    public string Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public XMSettingsRow() { }
    public XMSettingsRow(bool isComment, string key, string value)
    {
        _isComment = isComment;
        _key = key;
        _value = value;
    }

    public static XMSettingsRow Parse(string row)
    {
        XMSettingsRow xmRow = null;
        if (!string.IsNullOrEmpty(row))
        {
            xmRow = new XMSettingsRow();
            string[] strings = row.Split(new char[] { '\t' });
            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = strings[i].Trim();
            }
            if (strings[0][0] == '#')
            {
                xmRow.IsComment = true;
                xmRow.Key = strings[0].Substring(1).Trim(new char[] { '"' });
            }
            else
            {
                xmRow.IsComment = false;
                xmRow.Key = strings[0].Trim(new char[] { '"' });
            }
            if (strings.Length > 1)
            {
                xmRow.Value = strings[1].Trim(new char[] { '"' });
            }
        }
        return xmRow;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        if (IsComment)
        {
            sb.Append("#");
        }
        if (!string.IsNullOrEmpty(Key))
        {
            sb.AppendFormat("\"{0}\"", Key);
            if (!string.IsNullOrEmpty(Value))
            {
                sb.AppendFormat("\t\"{0}\"", Value);
            }
        }
        return sb.ToString();
    }

    #region IEquatable<XMSettingsRow> Members

    public bool Equals(XMSettingsRow other)
    {
        if (string.Compare(Key, other.Key, StringComparison.InvariantCultureIgnoreCase) == 0)
        {
            return true;
        }
        return false;
    }

    #endregion
}

/// <summary>
/// Summary description for XMServerSettings
/// </summary>
public class XMServerSettings
{
    protected List<XMSettingsRow> _rows = new List<XMSettingsRow>();
    protected XMLayer _xm;

    public List<XMSettingsRow> Rows
    {
        get { return _rows; }
    }

    public XMLayer Xm
    {
        get { return _xm; }
        set { _xm = value; }
    }

    public XMServerSettings()
    {
        _xm = XMServer.CreateXMLayer();
    }

    public static XMServerSettings CreateXMServerSettings()
    {
        if ((HttpContext.Current != null))
        {
            if (HttpContext.Current.Application[AdminPanelConstants.serverSettings] != null)
            {
                return (XMServerSettings)HttpContext.Current.Application[AdminPanelConstants.serverSettings];
            }
        }
        XMServerSettings settings = new XMServerSettings();
        settings._xm = XMServer.CreateXMLayer();
        settings.LoadSettings();
        if ((HttpContext.Current != null))
        {
            HttpContext.Current.Application.Add(AdminPanelConstants.serverSettings, settings);
        }
        return settings;
    }

    public void LoadSettings()
    {
        try
        {
            _xm.Login();
            string raw = _xm.GetConfigFile(AdminPanelConstants.serverTab);
            _xm.Logout();
            string line = string.Empty;

            using (StringReader sr = new StringReader(raw))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == ".") break;
                    XMSettingsRow row = XMSettingsRow.Parse(line);
                    if (row != null) _rows.Add(row);
                }
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
    }

    public void SaveSettings()
    {
        try
        {
            using (StringWriter sw = new StringWriter())
            {
                for (int i = 0; i < _rows.Count; i++)
                    sw.WriteLine(_rows[i]);

                string data = sw.ToString();
                _xm.Login();
                _xm.SetConfigFile(AdminPanelConstants.serverTab, data);
                _xm.Logout();
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
    }

    public XMSettingsRow GetXMSettingsRow(string key)
    {
        XMSettingsRow result = null;
        int index = 0;
        XMSettingsRow item = new XMSettingsRow();
        item.Key = key;
        while ((index >= 0) && (index < _rows.Count))
        {
            index = _rows.IndexOf(item, index);
            if (index <= 0) continue;
            result = _rows[index];
            if (!result.IsComment) break;
            index++;
        }
        return result;
    }

    public void SetXMSettingsRow(XMSettingsRow row)
    {
        int index = _rows.IndexOf(row);
        if (index >= 0)
        {
            _rows[index].IsComment = row.IsComment;
            _rows[index].Key = row.Key;
            _rows[index].Value = row.Value;
        }
        else
        {
            _rows.Add(row);
        }
    }
}

public interface IXMSettingsCtrl
{
    void InitControl(XMServerSettings xmSettings);
    void SaveSettings(ref XMServerSettings xmSettings);
}
