using System;
using System.Collections.Generic;
using XMailAdminProxy;
using System.Text.RegularExpressions;
using System.IO;
using WebMail;


public class AdminPanelControlAccounts
{
    public struct AdminPanelControlAccount
    {
        public string Name;
        public string Password;
    }

    public static string[] extractCtrlAccounts(string line)
    {
        string[] parts = null;
        
        Regex re = new Regex("\\s+");
        parts = re.Split(line);
        for (int i = 0; i < parts.Length; i++) parts[i] = parts[i].Trim(new char[] { '"' });
        return parts;
    }

    public static void AddControlAccount(AdminPanelControlAccount cAccount)
    {
        List<AdminPanelControlAccount> cAccounts = GetControlAccounts();
        cAccounts.Add(cAccount);
        SetControlAccounts(cAccounts);
    }

    public static void SetControlAccountPassword(string Name, string Password)
    {
        List<AdminPanelControlAccount> cAccountsNew = new List<AdminPanelControlAccount>();
        AdminPanelControlAccount cAccount = new AdminPanelControlAccount();
        List<AdminPanelControlAccount> cAccounts = GetControlAccounts();

        foreach (AdminPanelControlAccount ca in cAccounts)
        {
            cAccount = ca;
            if (ca.Name == Name) cAccount.Password = Password;
            cAccountsNew.Add(cAccount);
        }
        SetControlAccounts(cAccountsNew);
    }

    public static void DeleteControlAccount(AdminPanelControlAccount cAccount)
    {
        List<AdminPanelControlAccount> cAccounts = GetControlAccounts();
        List<AdminPanelControlAccount> cAccountsNew = new List<AdminPanelControlAccount>();

        foreach (AdminPanelControlAccount ca in cAccounts)
            if (ca.Name != cAccount.Name) cAccountsNew.Add(ca);

        SetControlAccounts(cAccountsNew);
    }

    public static List<AdminPanelControlAccount> GetControlAccounts()
    {
        List<AdminPanelControlAccount> cAccounts = new List<AdminPanelControlAccount>();
        try
        {
            XMLayer xm = XMServer.CreateXMLayer();
            string raw = string.Empty;
            string line;
            xm.Login();
            raw = xm.GetCtrlAccounts();
            xm.Logout();
            using (StringReader sr = new StringReader(raw))
            {
                string[] data;
                AdminPanelControlAccount cAccount = new AdminPanelControlAccount();
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == ".") break;
                    data = extractCtrlAccounts(line);
                    cAccount.Name = data[0];
                    cAccount.Password = AdminPanelUtils.DecryptPassword(data[1]);
                    cAccounts.Add(cAccount);
                }
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw new WebMail.WebMailException(error);
        }
        return cAccounts;
    }

    public static bool IsControlAccountExist(string Name)
    {
        List<AdminPanelControlAccount> cAccounts = new List<AdminPanelControlAccount>();
        bool result = false;
        try
        {
            cAccounts = AdminPanelControlAccounts.GetControlAccounts();
            foreach (AdminPanelControlAccount ca in cAccounts)
            {
                if (ca.Name != Name) continue;
                result = true;
                break;
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw new WebMail.WebMailException(error);
        }
        return result;
    }

    public static void SetControlAccounts(List<AdminPanelControlAccount> cAccounts)
    {
        try
        {
            XMLayer xm = XMServer.CreateXMLayer();
            string param = string.Empty;

            foreach (AdminPanelControlAccount ca in cAccounts)
                param += '\"' + ca.Name + '\"' + '\t' + '\"' + AdminPanelUtils.EncryptPassword(ca.Password) + '\"' + "\r\n";

            xm.Login();
            xm.SetCtrlAccounts(param);
            xm.Logout();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw new WebMail.WebMailException(error);
        }
    }
}
