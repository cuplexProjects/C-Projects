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
using WebMail;

public partial class CheckConnection : System.Web.UI.UserControl
{
    public bool _errorPOP3 = false;
    public bool _errorSMTP = false;
    public bool _errorIMAP = false;
    public string _connectionResult = string.Empty;
    public string _resultSMTP = string.Empty;
    public string _resultPOP3 = string.Empty;
    public string _resultIMAP = string.Empty;

    public string _errorMessage = string.Empty;

    protected int _web_step = 1;
    protected int _max_step = 7;

    protected void Page_Load(object sender, EventArgs e)
    {
        install _page = Page as install;
        _web_step = _page._web_step;
        _max_step = _page._max_step;

        object _EmptyKey = Session["EmptyKey"];
        if (_EmptyKey != null)
        {
            submit_btn.Attributes["onclick"] = "";
        }
        else
        {
            
            submit_btn1.Attributes["onclick"] = "window.open('http://www.afterlogic.com/congratulations/webmail-lite-net');";
        }
    }
    protected void NextButton_Click(object sender, System.EventArgs e)
    {
        Response.Redirect("install.aspx?mode=end", false);
    }

    protected void TestConnectionButton_Click(object sender, System.EventArgs e)
    {
        string host = txtHost.Value;
        if (chSMTP.Checked)
        {
            // SMTP
            try
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Connect(host, 25);
                s.Close();
                _resultSMTP = @"<font color=""green"">SMTP connection to port 25 successful, sending outgoing e-mail over SMTP should work.</font>";
            }
            catch (Exception error)
            {
                Log.WriteException(error);
                _errorSMTP = true;
                _resultSMTP = @"<font color=""red"">SMTP connection to port 25 failed: " + error.Message + ".</font>";
            }
            _connectionResult = _connectionResult + _resultSMTP + "<br/>";
        } 
        
        if (chPOP3.Checked)
        {
            // POP3
            try
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Connect(host, 110);
                s.Close();
                _resultPOP3 = @"<font color=""green"">POP3 connection to port 110 successful, checking and downloading incoming e-mail over POP3 should work.</font>";
            }
            catch (Exception error)
            {
                Log.WriteException(error);
                _errorPOP3 = true;
                _resultPOP3 = @"<font color=""red"">POP3 connection to port 110 failed: " + error.Message + ".</font>";
            }
            _connectionResult = _connectionResult + _resultPOP3 + "<br/>";
        }

        if (chIMAP4.Checked)
        {
            // IMAP
            try
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Connect(host, 143);
                s.Close();
                _resultIMAP = @"<font color=""green"">IMAP connection to port 143 successful, checking and downloading incoming e-mail over IMAP should work.</font>";
            }
            catch (Exception error)
            {
                Log.WriteException(error);
                _errorIMAP = true;
                _resultIMAP = @"<font color=""red"">IMAP4 connection to port 143 failed: " + error.Message + ".</font>";
            }
            _connectionResult = _connectionResult + _resultIMAP;
        }

        if (_errorPOP3 || _errorSMTP || _errorIMAP)
        {
            _errorMessage = "<br />Check your firewall settings and e-mail server configuration, make sure the e-mail server host name is spelled correctly and the computer running this installer can access the e-mail server over the required ports.";
        }
    }

}
