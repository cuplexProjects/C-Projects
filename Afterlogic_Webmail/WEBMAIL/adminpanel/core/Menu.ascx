<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="WebMail.adminpanel.core.Menu" %>

<asp:Literal ID="PluginsMenu" runat="server"></asp:Literal>
<div class="wm_accountslist_logout">
    <a href="default.aspx?mode=logout">Logout</a>
</div>
<div class="wm_accountslist_settings">
    <a href="default.aspx?mode=help" target=_blank>Help</a>
</div>
