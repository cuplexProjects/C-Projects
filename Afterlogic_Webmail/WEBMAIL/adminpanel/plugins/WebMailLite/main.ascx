<%@ Control Language="C#" AutoEventWireup="true" Inherits="PlugIns_WebMailLite_main" Codebehind="main.ascx.cs" %>
<%@ Register Src="Controls/menu.ascx" TagPrefix="MailAdm" TagName="Menu" %>
<%@ Register Src="Controls/database.ascx" TagPrefix="MailAdm" TagName="Database" %>
<%@ Register Src="Controls/debug.ascx" TagPrefix="MailAdm" TagName="Debug" %>
<%@ Register Src="Controls/interface.ascx" TagPrefix="MailAdm" TagName="Interface" %>
<%@ Register Src="Controls/login.ascx" TagPrefix="MailAdm" TagName="Login" %>
<%@ Register Src="Controls/webmail.ascx" TagPrefix="MailAdm" TagName="WebMail" %>
<%@ Register Src="Controls/server-integration.ascx" TagPrefix="MailAdm" TagName="Server" %>

<div class="wm_settings">
	<div class="wm_settings_row">
		<div class="wm_settings_cont" style="height: auto;">
			<asp:PlaceHolder ID="ContentPlaceHolder" runat="server"></asp:PlaceHolder>
		</div>
		<div class="wm_settings_nav" style="height: auto;">
			<mailadm:menu id="MenuID" runat="server"></mailadm:menu>
		</div>
		<div class="clear">
		</div>
	</div>
</div>
