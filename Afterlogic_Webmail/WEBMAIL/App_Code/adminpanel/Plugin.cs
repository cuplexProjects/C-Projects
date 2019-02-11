using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.SessionState;
using System.Collections.Generic;
using WebMail;
using System.IO;
using System.Xml.XPath;
using System.Xml;
using System.Collections;
using System.Globalization;

public class AdminPanelMainPlugin : System.Web.UI.UserControl 
{
    public string PluginID;

    public void SetPluginID(string pID)
    {
        PluginID = pID;
    }

    public virtual void InitPlugin()
    {
    }

    public virtual bool CanLoadPlugin()
    {
        return false;
    }
}

public class Plugin
{
    protected string _id;
    protected string _caption;
    protected string _target;
    protected string _folderName;

    public string ID
    {
        get { return _id; }
        set { _id = value; }
    }

    public string Caption
    {
        get { return _caption; }
        set { _caption = value; }
    }

    public string Target
    {
        get { return _target; }
        set { _target = value; }
    }

    public string FolderName
    {
        get { return _folderName; }
        set { _folderName = value; }
    }

    public static PluginCollection GetPlugins(string rootFolder, HttpSessionState session, AdminPanelSettings apSettings)
    {
        PluginCollection result = new PluginCollection();

        try
        {
            foreach (Plugin pl in apSettings.Plugins)
            {
                if (Directory.Exists(Path.Combine(rootFolder, @"plugins\" + pl.FolderName)))
                {
                    result.Add(pl);
                }
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }

        return result;
    }

    public static Plugin GetPlugin(string plugin, string rootFolder, AdminPanelSettings apSettings)
    {
        Plugin result = null;
        try
        {
            result = apSettings.Plugins[plugin];

            if (result != null)
            {
                if (!Directory.Exists(Path.Combine(rootFolder, "plugins/" + result.FolderName)))
                {
                    result = null;
                }
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            throw;
        }
        return result;
    }

    public static bool IsPluginExist(string PlugName, AdminPanelSettings apSettings)
    {
        if (apSettings.Plugins[PlugName] != null)
        {
            return true;
        }
        return false;
    }

    public static bool IsPluginExist(string rootFolder, string PlugName)
    {
        try
        {
            if (Directory.Exists(Path.Combine(rootFolder, "plugins/" + PlugName)))
                return true;
        }
        catch (Exception ex)
        {
            Log.WriteException(ex);
            throw;
        }
        return false;
    }
}

public class PluginCollection : CollectionBase
{
    public PluginCollection() { }

    public Plugin this[int index]
    {
        get { return (Plugin)List[index]; }
        set { List[index] = value; }
    }

    public Plugin this[string ID]
    {
        get
        {
            foreach (Plugin plugin in List)
            {
                if (string.Compare(plugin.ID, ID, true, CultureInfo.InvariantCulture) == 0)
                {
                    return plugin;
                }
            }
            return null;
        }
    }

    public int Add(Plugin plugin)
    {
        return List.Add(plugin);
    }

    public int IndexOf(Plugin plugin)
    {
        return List.IndexOf(plugin);
    }

    public int IndexOf(string ID)
    {
        Plugin plugin = this[ID];
        return List.IndexOf(plugin);
    }

    public void Insert(int index, Plugin plugin)
    {
        List.Insert(index, plugin);
    }

    public void Remove(Plugin plugin)
    {
        List.Remove(plugin);
    }

    public bool Contains(Plugin plugin)
    {
        return List.Contains(plugin);
    }
}