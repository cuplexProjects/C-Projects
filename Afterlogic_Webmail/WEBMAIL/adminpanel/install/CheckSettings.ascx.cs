using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net.Sockets;
using System.IO;
using System.Data.Odbc;
using WebMail;

public partial class CheckSettings : System.Web.UI.UserControl
{
    public string _environmentVersion = string.Empty;
    public string _OSVersion = string.Empty;
    
    public string _Sessions = "";
    public string _SessionsMessage = "";

    public string _Sockets = "";
    public string _SocketsMessage = "";

    public string _ODBC = "";
    public string _ODBCMessage = "";
    
    public string _DataFolderLocation = "";
    public string _DataFolderLocationMessage = "";

    public string _CreatingDeletingFolders = "";
    public string _CreatingDeletingFoldersMessage = "";
    
    public string _CreatingDeletingFiles = "";
    public string _CreatingDeletingFilesMessage = "";
    
    public string _AdminPanelSettingsFileLocation = "";
    public string _AdminPanelReadSettingsFile = "";
    public string _AdminPanelWriteSettingsFile = "";
    public string _AdminPanelReadWriteSettingsFileMessage = "";
    public string _AdminPanelSettingsFileLocationMessage = "";

    public string _WebMailSettingsFileLocation = "";
    public string _WebMailReadSettingsFile = "";
    public string _WebMailWriteSettingsFile = "";
    public string _WebMailReadWriteSettingsFileMessage = "";
    public string _WebMailSettingsFileLocationMessage = "";

    public string _DataFolderLocationStyle = "";
    public string _AdminPanelSettingsFileLocationStyle = "";
    public string _WebMailSettingsFileLocationStyle = "";

    public string _ResultMessage = "";
    
    public bool _Error = false;
    public string btnCheckName = "Next";
    public string btnCheckOnClick = "";

    protected string var_sess = "0";
    protected string var_dfpath = "0";
    protected string var_cdfolders = "0";
    protected string var_cdfiles = "0";
    protected string var_apsfl = "0";
    protected string var_apsfread = "0";
    protected string var_apsfwrite = "0";
    protected string var_wmsfl = "0";
    protected string var_wmsfread = "0";
    protected string var_wmsfwrite = "0";
    protected string var_odbc = "0";
    protected string var_sockets = "0";

    public string var_str = "";

    protected int _web_step = 1;
    protected int _max_step = 7;

    protected void Page_Load(object sender, EventArgs e)
    {
        install _page = Page as install;
        _web_step = _page._web_step;
        _max_step = _page._max_step;

        try
        {
            Session["TestCreateSessions"] = "1";
            _Sessions = @"<font color=""green"">OK</font>";
            var_sess = "1";
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            _Sessions = @"<font color=""red"">Error, session support in ASP.NET must be enabled</font>";
            _SessionsMessage = "<br/><br/>You need to enable session support in properties of WebMail and AdminPanel virtual directories / ASP.NET tab / Edit Configuration / State Management tab / Session State mode in your IIS Manager. Set it to any value, except Off. InProc is recommended value in most cases. Alternatively, you may manually enable session support in sessionState tag of web.config files in WebMail and AdminPanel folders. In case of a shared hosting, changing sessionState may have no effect if your ISP prohibited changing this parameter. You need to ask your hosting provider to enable session support in this case.";
            _Error = true;
        }

        string DataFolderPath = AdminPanelUtils.GetWebMailDataFolder();

        if (DataFolderPath != null && Directory.Exists(DataFolderPath))
        {
            _DataFolderLocation = @"<font color=""green"">Found</font>";
            var_dfpath = "1";
            try
            {
                string temp_dir = Path.Combine(DataFolderPath, Path.GetRandomFileName());
                DirectoryInfo di = Directory.CreateDirectory(temp_dir);
                di.Delete();
                _CreatingDeletingFolders = @"<font color=""green"">OK</font>";
                var_cdfolders = "1";
            }
            catch (Exception)
            {
                _Error = true;
                _CreatingDeletingFolders = @"<font color=""red"">Creating/deleting folders Error, can't create or delete sub-folders in the data folder.</font>";
                
                
                _CreatingDeletingFoldersMessage = @"<br/><br/>You need to grant read/write permission over WebMail Lite data folder and all its contents to your web server user. For instructions, please refer to <a href=""docs/index-installation-instructions.htm"">this</a> section of WebMail Lite documentation and our <a href=""http://www.afterlogic.com/support/faq-webmail-lite-asp-net#3"" target=_blank>FAQ</a>.";
                
            }

            try
            {
                string temp_file = Path.Combine(DataFolderPath, Path.GetRandomFileName());
                FileStream fs = File.Create(temp_file);
                fs.Close();
                if (File.Exists(temp_file))
                {
                    File.Delete(temp_file);
                }
                _CreatingDeletingFiles = @"<font color=""green"">OK</font>";
                var_cdfiles = "1";
            }
            catch (Exception)
            {
                _Error = true;
                _CreatingDeletingFiles = @"<font color=""red"">Creating/deleting files Error, can't create files in the data folder.</font>";
                
                
                _CreatingDeletingFilesMessage = @"<br/><br/>You need to grant read/write permission over WebMail Lite data folder and all its contents to your web server user. For instructions, please refer to <a href=""docs/index-installation-instructions.htm"">this</a> section of WebMail Lite documentation and our <a href=""http://www.afterlogic.com/support/faq-webmail-lite-asp-net#3"" target=_blank>FAQ</a>.";
                
            }

            if (File.Exists(Path.Combine(DataFolderPath, @"settings\adminpanel.xml")))
            {
                _AdminPanelSettingsFileLocation = @"<font color=""green"">Found</font>";
                var_apsfl = "1";

                try
                {
                    FileStream _fs2 = File.OpenRead(Path.Combine(DataFolderPath, @"settings\adminpanel.xml"));
                    _fs2.Close();
                    _AdminPanelReadSettingsFile = @"<font color=""green"">OK</font>";
                    var_apsfread = "1";
                }
                catch (Exception)
                {
                    _Error = true;
                    _AdminPanelReadSettingsFile = @"<font color=""red"">Error, can't read <nobr>""" + DataFolderPath + @"settings\adminpanel.xml" + @"""</nobr> file</font>";
                    
                    
                    _AdminPanelReadWriteSettingsFileMessage = @"<br/><br/>You should grant read/write permission over WebMail Lite admin panel settings file to your web server user. For instructions, please refer to <a href=""docs/index-installation-instructions.htm"">this</a> section of WebMail documentation and our <a href=""http://www.afterlogic.com/support/faq-webmail-lite-asp-net#3"" target=_blank>FAQ</a>.";
                    
                }
                try
                {
                    FileStream _fs2 = File.OpenWrite(Path.Combine(DataFolderPath, @"settings\adminpanel.xml"));
                    _fs2.Close();
                    _AdminPanelWriteSettingsFile = @"<font color=""green"">OK</font>";
                    var_apsfwrite = "1";
                }
                catch (Exception)
                {
                    _Error = true;
                    _AdminPanelWriteSettingsFile = @"<font color=""red"">Error, can't write <nobr>""" + DataFolderPath + @"settings\adminpanel.xml" + @"""</nobr> file</font>";
                    
                    
                    _AdminPanelReadWriteSettingsFileMessage = @"<br/><br/>You should grant read/write permission over WebMail Lite admin panel settings file to your web server user. For instructions, please refer to <a href=""docs/index-installation-instructions.htm"">this</a> section of WebMail documentation and our <a href=""http://www.afterlogic.com/support/faq-webmail-lite-asp-net#3"" target=_blank>FAQ</a>.";
                    
                }
            }
            else
            {
                _Error = true;
                _AdminPanelSettingsFileLocation = @"<font color=""red"">Not Found, can't find <nobr>""" + DataFolderPath + @"settings\adminpanel.xml""</nobr> file.</font>";

                tr_AdminPanelReadSettingsFile.Attributes["class"] = "wm_hide";

                
                
                _AdminPanelSettingsFileLocationMessage = @"<br/><br/>Make sure you completely copied the data folder with all its contents from WebMail Lite installation package into the location you specified in web.config file.";
                
                _AdminPanelSettingsFileLocationStyle = "border:1px solid #ff0000";
            }

            if (File.Exists(Path.Combine(DataFolderPath, @"settings\settings.xml")))
            {
                _WebMailSettingsFileLocation = @"<font color=""green"">Found</font>";
                var_wmsfl = "1";

                try
                {
                    FileStream _fs1 = File.OpenRead(Path.Combine(DataFolderPath, @"settings\settings.xml"));
                    _fs1.Close();
                    _WebMailReadSettingsFile = @"<font color=""green"">OK</font>";
                    var_wmsfread = "1";
                }
                catch (Exception)
                {
                    _Error = true;
                    _WebMailReadSettingsFile = @"<font color=""red"">Error, can't read <nobr>""" + DataFolderPath + @"settings\settings.xml" + @"""</nobr> file</font>";
                    
                    
                    _WebMailReadWriteSettingsFileMessage = @"<br/><br/>You should grant read/write permission over WebMail Lite settings file to your web server user. For instructions, please refer to <a href=""docs/index-installation-instructions.htm"">this</a> section of WebMail documentation and our <a href=""http://www.afterlogic.com/support/faq-webmail-lite-asp-net#3"" target=_blank>FAQ</a>.";
                    
                }

                try
                {
                    FileStream _fs1 = File.OpenWrite(Path.Combine(DataFolderPath, @"settings\settings.xml"));
                    _fs1.Close();
                    _WebMailWriteSettingsFile = @"<font color=""green"">OK</font>";
                    var_wmsfwrite = "1";
                }
                catch (Exception)
                {
                    _Error = true;
                    _WebMailWriteSettingsFile = @"<font color=""red"">Error, can't write <nobr>""" + DataFolderPath + @"settings\settings.xml" + @"""</nobr> file</font>";
                    
                    
                    _WebMailReadWriteSettingsFileMessage = @"<br/><br/>You should grant read/write permission over WebMail Lite settings file to your web server user. For instructions, please refer to <a href=""docs/index-installation-instructions.htm"">this</a> section of WebMail documentation and our <a href=""http://www.afterlogic.com/support/faq-webmail-lite-asp-net#3"" target=_blank>FAQ</a>.";
                    
                }
            }
            else
            {
                _Error = true;
                _WebMailSettingsFileLocation = @"<font color=""red"">Not Found, can't find <nobr>""" + DataFolderPath + @"settings\settings.xml""</nobr> file.</font>";

                tr_WebMailReadSettingsFile.Attributes["class"] = "wm_hide";

                
                
                _WebMailSettingsFileLocationMessage = @"<br/><br/>Make sure you completely copied the data folder with all its contents from WebMail Lite installation package into the location you specified in web.config file.";
                
                _WebMailSettingsFileLocationStyle = "border:1px solid #ff0000";
            }
        }
        else
        {
            _Error = true;

            _DataFolderLocation = @"<font color=""red"">Not Found, can't find <nobr>"""+DataFolderPath+@"""</nobr> folder.</font>";

            
            
            _DataFolderLocationMessage = @"<br/><br/>You need to specify correct path to the data folder in web.config file (resides in web folder of WebMail Lite). You can learn more from our <a href=""http://www.afterlogic.com/support/faq-webmail-lite-asp-net#2"" target=_blank>FAQ</a>";
            
            _DataFolderLocationStyle = "border:1px solid #ff0000";

            tr_AdminPanelReadSettingsFile.Attributes["class"] = "wm_hide";
            tr_AdminPanelSettingsFileLocation.Attributes["class"] = "wm_hide";
            tr_WebMailReadSettingsFile.Attributes["class"] = "wm_hide";
            tr_WebMailSettingsFileLocation.Attributes["class"] = "wm_hide";
            tr_CreatingDeletingFiles.Attributes["class"] = "wm_hide";
            tr_CreatingDeletingFolders.Attributes["class"] = "wm_hide";
        }

        try
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.ReceiveTimeout = 1;
            s.Connect("localhost", 80);
            _Sockets = @"<font color=""green"">OK</font>";
            var_sockets = "1";
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            if (error is System.Security.SecurityException)
            {
                _Sockets = @"<font color=""red"">No SocketPermission</font>";
                _SocketsMessage = "<br/>This means using sockets is prohibited. You should increase \"trust level\" in your ASP.NET configuration to \"Hight Trust\" or \"Full Trust\" or customize the current \"trust level\" to enable SocketPermission.<br/>To learn more, please refer to <a href=\"http://www.afterlogic.com/kb/articles/webmail-pro-aspnet-no-socketpermission-or-no-odbcpermission-errors-and-medium-trust-policy\" target=_blank>this</a> KB article.";
                _Error = true;
            }
            else
            {
                _Sockets = @"<font color=""green"">OK</font>";
                var_sockets = "1";
            }
        }

        try
        {
            OdbcConnection odbc1 = new OdbcConnection();
            odbc1.ConnectionTimeout = 1;
            odbc1.ConnectionString = @"Driver=MySQL ODBC 3.51 Driver;Server=localhost;Port=3306;Database=webmail;User=user;Password=pass;Option=3;";
            odbc1.Open();
            _ODBC = @"<font color=""green"">OK</font>";
            var_odbc = "1";
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            if (error is System.Security.SecurityException)
            {
                _ODBC = @"<font color=""red"">No OdbcPermission</font>";
                _ODBCMessage = "<br/>This means using ODBC driver is not allowed. You should increase \"trust level\" in your ASP.NET configuration to \"Hight Trust\" or \"Full Trust\" or customize the current \"trust level\" to enable OdbcPermission.<br/>To learn more, please refer to <a href=\"http://www.afterlogic.com/kb/articles/webmail-pro-aspnet-no-socketpermission-or-no-odbcpermission-errors-and-medium-trust-policy\" target=_blank>this</a> KB article.";
                _Error = true;
            }
            else
            {
                _ODBC = @"<font color=""green"">OK</font>";
                var_odbc = "1";
            }
        }

        if (_Error)
        {
            btnCheckName = "Retry";
            btnCheckOnClick = "document.location='install.aspx?mode=check'";
            _divResult.Attributes["class"] = "wm_install_last_div_error";
            _ResultMessage = "Please make sure that all the requirements are met and click Retry.";
        }
        else
        {
            btnCheckName = "Next";
            btnCheckOnClick = "document.location='install.aspx?mode=license'";
            _divResult.Attributes["class"] = "wm_install_last_div_ok";
            _ResultMessage = "The current server environment meets all the requirements. Click Next to proceed.";
        }

        var_str = "&var_sess=" + var_sess +
                  "&var_dfpath=" + var_dfpath +
                  "&var_cdfolders=" + var_cdfolders +
                  "&var_cdfiles=" + var_cdfiles +
                  "&var_apsfl=" + var_apsfl +
                  "&var_apsfread=" + var_apsfread +
                  "&var_apsfwrite=" + var_apsfwrite +
                  "&var_wmsfl=" + var_wmsfl +
                  "&var_wmsfread=" + var_wmsfread +
                  "&var_wmsfwrite=" + var_wmsfwrite +
                  "&var_sockets=" + var_sockets +
                  "&var_odbc=" + var_odbc; 
        
        if (Session["EmptyKey"] == null)
        {
            _page.check_str = var_str;
        }
    }
}
