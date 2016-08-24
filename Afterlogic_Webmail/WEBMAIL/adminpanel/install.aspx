<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="install.aspx.cs" Inherits="install" %>

<%@ Register Src="install/CheckSettings.ascx" TagPrefix="install" TagName="CheckSettings" %>
<%@ Register Src="install/License.ascx" TagPrefix="install" TagName="License" %>
<%@ Register Src="install/LicenseKeySettings.ascx" TagPrefix="install" TagName="LicenseKeySettings" %>
<%@ Register Src="install/DatabaseSettings.ascx" TagPrefix="install" TagName="DatabaseSettings" %>
<%@ Register Src="install/CommonSettings.ascx" TagPrefix="install" TagName="CommonSettings" %>
<%@ Register Src="install/EndOfInstallation.ascx" TagPrefix="install" TagName="EndOfInstallation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="Expires" content="0" />
	<meta http-equiv="Cache-Control" content="no-cache" />
	<meta http-equiv="Pragma" content="no-cache" />
	<meta http-equiv="Content-Script-Type" content="text/javascript" />
	<title><%=_Title%> — AfterLogic WebMail Lite Installation</title>
	<link rel="stylesheet" href="styles/styles.css" type="text/css" />

	<script language="javascript" type="text/javascript" src="js/errorShow.js"></script>
	<script language="javascript" type="text/javascript" src="js/tooltip.js"></script>
	<script language="javascript" type="text/javascript" src="js/common.js"></script>
	<script language="javascript" type="text/javascript" src="js/reports.js"></script>
    <script language="javascript" type="text/javascript" src="js/functions.js"></script>
    <script language="javascript" type="text/javascript" src="js/install.js"></script>
</head>
<body>
	<form id="installForm" runat="server">
	<asp:HiddenField runat="server" ID="errorMessage" Value="" />
	<asp:HiddenField runat="server" ID="reportMessage" Value="" />
	
	<div class="wm_content_webmaillite" align="center">
		<div class="wm_logo" id="logo"></div>
		<div style="background-color: #fff;">
            <table class="wm_settings" cellpadding="0" cellspacing="0">
	            <tr>
		            <td valign="top" align="left" class="wm_install_nav">
	                    <br />
                        <div>
                            <img style="width:100px; height:75px; margin: 0px 0px 0px 15px;" src="<%=_img_url%><%=check_str%>" />
                        </div>	                    
	                    <div id="menuCheck" runat="server">
		                    <%=_Check%>
	                    </div>
	                    <div id="menuLicense" runat="server">
		                    <%=_License%>
	                    </div>
	                    <div id="menuLicenseKey" runat="server">
		                    <%=_LicenseKey%>
	                    </div>
	                    <div id="menuDB" runat="server">
		                    <%=_DB%>
	                    </div>
	                    <div id="menuCommon" runat="server">
		                    <%=_Common%>
	                    </div>
	                    <div id="menuCheckConnection" runat="server">
		                    <%=_CheckConnection%>
	                    </div>
	                    
	                    <div id="menuEnd" runat="server">
		                    <%=_Finish%>
	                    </div>
		            </td>
		            <td style="vertical-align: top" class="wm_settings_cont" align="right">
                        <asp:PlaceHolder ID="ContentPlaceHolder" runat="server"></asp:PlaceHolder>
		            </td>
                </tr>		
            </table>
		</div>
		<br />
        <div id="copyright" class="wm_copyright">
            Copyright © 2002-2010 <a target="_blank" href="http://www.afterlogic.com/">AfterLogic Corporation</a>
        </div>
	</div>
	</form>
</body>
<script type="text/javascript">
	Tip = new CTip();

    var errorMessage = document.getElementById("errorMessage");
    var reportMessage = document.getElementById("reportMessage");
    if(errorMessage.value != "")
    {
        MsgBox.Show(errorMessage.value, 2);
        errorMessage.value = "";
    }	
    if(reportMessage.value != "")
    {
        MsgBox.Show(reportMessage.value, 1);
        reportMessage.value = "";
    }
</script>
</html>

