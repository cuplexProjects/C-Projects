using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using WebMail;

/// <summary>
/// Summary description for AdminPanelSettings
/// </summary>
public class AdminPanelSettings
{
    #region Fields
    
    protected AdminPanelSettings _instance;

    public PluginCollection _Plugins;
    protected string _User;
    protected string _Pass;
    protected string _Host;
    protected string _Port;
    protected string _LicenseKey;
    protected bool _AdvancedOptions;

    protected bool _Loaded;

    #endregion

    #region Properties

    public PluginCollection Plugins
    {
        get { return _Plugins; }
        set { _Plugins = value; }
    }

    public string User
    {
        get { return _User; }
        set { _User = value; }
    }

    public string Pass
    {
        get { return _Pass; }
        set { _Pass = value; }
    }

    public string Host
    {
        get { return _Host; }
        set { _Host = value; }
    }

    public string Port
    {
        get { return _Port; }
        set { _Port = value; }
    }

    public string LicenseKey
    {
        get { return _LicenseKey; }
        set { _LicenseKey = value; }
    }

    public bool AdvancedOptions
    {
        get { return _AdvancedOptions; }
        set { _AdvancedOptions = value; }
    }

    public bool Loaded
    {
        get { return _Loaded; }
        set { _Loaded = value; }
    }

    #endregion

    public AdminPanelSettings()
    {
        _Plugins = new PluginCollection();
        _User = string.Empty;
        _Pass = string.Empty;
        _Host = string.Empty;
        _Port = string.Empty;
        _LicenseKey = string.Empty;
        _AdvancedOptions = false;
        _Loaded = false;
    }

    public AdminPanelSettings CreateInstance()
    {
        if (_instance == null)
        {
            _instance = new AdminPanelSettings();
            _instance.Load();
        }

        return _instance;
    }

    public void LoadXML(XmlNode root)
    {
        for (int i = 0; i < root.ChildNodes.Count; i++)
        {
            XmlNode node = root.ChildNodes[i];
            switch (node.Name)
            {
                case "plugins":
                case "login":
                    if (node.ChildNodes.Count > 0) LoadXML(node);
                    break;
                case "plugin":
                    Plugin pl = new Plugin();
                    if (node.Attributes["caption"] != null)
                    {
                        pl.Caption = node.Attributes["caption"].Value;
                    }
                    if (node.Attributes["folderName"] != null)
                    {
                        pl.FolderName = node.Attributes["folderName"].Value;
                    }
                    if (node.Attributes["target"] != null)
                    {
                        pl.Target = node.Attributes["target"].Value;
                    }
                    pl.ID = pl.FolderName; 
                    Plugins.Add(pl);
                    break;
                case "user":
                    User = node.InnerText;
                    break;
                case "password":
                    Pass = node.InnerText;
                    break;
                case "host":
                    Host = node.InnerText;
                    break;
                case "port":
                    Port = node.InnerText;
                    break;
                case "licensekey":
                    LicenseKey = node.InnerText;
                    break;
                case "advancedoptions":
                    AdvancedOptions = (node.InnerText == "1") ? true : false;
                    break;
            }
        }
    }

    
    public AdminPanelSettings Load()
    {
        Load(AdminPanelUtils.GetAdminPanelDataFolderPath());
        return this;
    }

    public AdminPanelSettings Load(string dataFolder)
    {
        string filename = Path.Combine(dataFolder, @"adminpanel.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(filename);
        XmlNode root = doc.DocumentElement;
        LoadXML(root);
        _Loaded = true;
        return this;
    }

    public AdminPanelSettings LoadFromString(string xml)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(new StringReader(xml));
        XmlNode root = doc.DocumentElement;
        LoadXML(root);
        _Loaded = true;
        return this;
    }

    public void Save()
    {
        Save(AdminPanelUtils.GetAdminPanelDataFolderPath());
    }

    public void Save(string dataFolder)
    {
        try
        {
            string filename = Path.Combine(dataFolder, @"adminpanel.xml");

            XmlDocument result = new XmlDocument();
            result.PreserveWhitespace = false;
            XmlDeclaration xmlDecl = result.CreateXmlDeclaration("1.0", "utf-8", "");
            result.AppendChild(xmlDecl);

            XmlElement settingsElem = result.CreateElement("settings");
            result.AppendChild(settingsElem);

            XmlElement subElemPlugins = result.CreateElement("plugins");
            settingsElem.AppendChild(subElemPlugins);

            XmlElement subElemLogin = result.CreateElement("login");
            settingsElem.AppendChild(subElemLogin);

            XmlElement subElem = result.CreateElement("user");
            subElem.AppendChild(result.CreateTextNode(User));
            subElemLogin.AppendChild(subElem);

            subElem = result.CreateElement("password");
            subElem.AppendChild(result.CreateTextNode(Pass));
            subElemLogin.AppendChild(subElem);

            subElem = result.CreateElement("host");
            subElem.AppendChild(result.CreateTextNode(Host));
            subElemLogin.AppendChild(subElem);
            
            subElem = result.CreateElement("port");
            subElem.AppendChild(result.CreateTextNode(Port));
            subElemLogin.AppendChild(subElem);

            foreach (Plugin pl in Plugins)
            {
                subElem = result.CreateElement("plugin");
                subElem.SetAttribute("caption", pl.Caption);
                subElem.SetAttribute("folderName", pl.FolderName);
                if (!string.IsNullOrEmpty(pl.Target))
                {
                    subElem.SetAttribute("target", pl.Target);
                }
                subElemPlugins.AppendChild(subElem);
            }

            XmlElement subElemLicenseKey = result.CreateElement("licensekey");
            subElemLicenseKey.AppendChild(result.CreateTextNode(LicenseKey));
            settingsElem.AppendChild(subElemLicenseKey);

            XmlElement subElemAdvancedOptions = result.CreateElement("advancedoptions");
            subElemAdvancedOptions.AppendChild(result.CreateTextNode(AdvancedOptions ? "1" : "0"));
            settingsElem.AppendChild(subElemAdvancedOptions);

            result.Save(filename);
            _instance = null;
        }
        catch (Exception error)
        {
            Log.WriteException(error);
        }
    }


}
