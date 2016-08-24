<%@ Control Language="C#" AutoEventWireup="true" Inherits="server_integrationLite" Codebehind="server-integration.ascx.cs" %>

<table class="wm_settings_common" width="550" border="0">
	<tr>
		<td width="100"></td>
		<td width="160"></td>
		<td></td>
	</tr>
	<tr>
		<td colspan="3" class="wm_settings_list_select"><b>Server Integration</b></td>
	</tr>
	<tr>
		<td colspan="3"><br>
		</td>
	</tr>
	<tr>
		<td align="right">Path to Server:
		</td>
		<td colspan="2">
			<input runat="server" type="text" id="txtWmServerRootPath" name="txtWmServerRootPath" value="C:\Program Files\AfterLogic XMail Server\MailRoot\"
				size="50" class="wm_input" maxlength="500">
		</td>
	</tr>
	<tr>
		<td align="right">
		</td>
		<td colspan="2">
			<div class="wm_safety_info">
				Path to the MailRoot folder of AfterLogic XMail Server in your system, for instance C:\Program Files\AfterLogic XMail Server\MailRoot\.
			</div>
		</td>
	</tr>
	
	<tr>
		<td align="right">Server Host: </td>
		<td colspan="2">
			<input type="text" runat="server" name="txtWmServerHostName" id="txtWmServerHostName" value="127.0.0.1" size="50" class="wm_input" maxlength="500">
		</td>
	</tr>
	<tr>
		<td align="right">
		</td>
	    <td colspan="2">
            <div class="wm_safety_info">
                IP address or hostname where AfterLogic XMail Server resides.
            </div>
            <br />
	    </td>
    </tr>
	<tr>
		<td align="right">
			<input type="checkbox"  runat="server" name="intWmAllowManageXMailAccounts" id="intWmAllowManageXMailAccounts" value="1" />
		</td>
		<td colspan="2">
		    <label for="<%=intWmAllowManageXMailAccounts.ClientID%>">
		        Allow&nbsp;users&nbsp;to&nbsp;manage&nbsp;accounts&nbsp;on&nbsp;AfterLogic&nbsp;XMail&nbsp;Server
		    </label>
		</td>
	</tr>
	<tr>
		<td align="right">
		</td>
	    <td colspan="2">
            <div class="wm_safety_info">
                If a user adds or removes a linked account in his primary account settings and domain part of this account matches any of
                your domains hosted by AfterLogic XMail Server, this account will be added/removed on AfterLogic XMail Server.
            </div>
    	</td>
    </tr>
	<!-- hr -->
	<tr>
		<td colspan="3">
		    <hr size="1">
		</td>
	</tr>
	<tr>
		<td></td>
		<td colspan="2" align="right">
		    <input runat="server" type="button" id="save" name="save" class="wm_button" value="Save"
				style="WIDTH: 100px" onserverclick="save_ServerClick">
		</td>
	</tr>
</table>

