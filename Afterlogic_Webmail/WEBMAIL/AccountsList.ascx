<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AccountsList.ascx.cs" Inherits="WebMail.classic.AccountsList" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table class="wm_accountslist" id="accountslist">
	<tbody>
		<tr>
			<td>
				<%=AcctList%>
				<%=_ContactsDiv%>
				<div class="wm_accountslist_logout"><a href="<%=_logoutLink%>"><%=_manager.GetString("Logout")%></a></div>
				<div class="wm_accountslist_settings<%=settingsActiveClassName%>"><a href="<%=_settingsLink%>"><%=_manager.GetString("Settings")%></a></div>
				<%=_CalendarDiv%>
			</td>
		</tr>
	</tbody>
</table>
