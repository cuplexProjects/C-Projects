<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminPanel.ascx.cs" Inherits="WebMail.adminpanel.AdminPanel" %>

<div class="wm_content" align="center">
	<div class="wm_logo" id="logo"></div>
	<div class="wm_accountslist" id="accountslist">
		<asp:PlaceHolder ID="menuPlaceHolder" runat="server"></asp:PlaceHolder>
	</div>
	<div style="background-color: #fff;">
		<asp:PlaceHolder ID="contentPlaceHolder" runat="server"></asp:PlaceHolder>
	</div>
</div>
