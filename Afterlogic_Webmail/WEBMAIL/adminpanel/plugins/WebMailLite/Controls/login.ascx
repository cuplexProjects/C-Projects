<%@ Control Language="C#" AutoEventWireup="true" Inherits="login_settingsLite" Codebehind="login.ascx.cs" %>
<script src="plugins/WebMailLite/js/login.js" type="text/javascript"></script>
<table class="wm_settings_common" width="550">
	<tr>
		<td width="50"></td>
		<td></td>
	</tr>
	<tr>
		<td colspan="2" class="wm_settings_list_select">
			<b>Login Settings</b>
		</td>
	</tr>
	<tr><td colspan="2"><br /></td></tr>
	<tr>
		<td></td>
		<td style="padding: 0px;">
			<div class="wm_safety_info">
				<b>Standard login panel, Hide login field, Hide email field</b>	are sorts of WebMail login panel.
				Choice depends on your mail server configuration and your requirements.
			</div>
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<br />
			<br />
			<div id="hideLoginDiv1">
				<div class="back" style="margin: 0px 2px; height: 1px; border: 0px; line-height: 1px; $height: auto;">&nbsp;</div>
				<div class="back" style="margin: 0px 1px; height: 1px; border: 0px; line-height: 1px; $height: auto;">&nbsp;</div>
				<table class="back" style="width: 100%" cellpadding="0" cellspacing="0">
					<tr>
						<td style="width: 40px;" valign="top" align="right">
                <input id="standartLoginRadio" style="VERTICAL-ALIGN: middle" onclick="StandartLoginRadioClick()"
					type="radio" runat="server" />            
						</td>
						<td align="left">
                <label for="<%=standartLoginRadio.ClientID%>"">Standard login panel</label>
							<br />
							<br />
						</td>
					</tr>
				</table>
				<div class="back" style="margin: 0px 1px; height: 1px; border: 0px; line-height: 1px; $height: auto;">&nbsp;</div>
				<div class="back" style="margin: 0px 2px; height: 1px; border: 0px; line-height: 1px; $height: auto;">&nbsp;</div>
			</div>
			
			<div id="hideLoginDiv2">
				<div class="back" style="margin: 0px 2px; height: 1px; border: 0px; line-height: 1px; $height: auto;">&nbsp;</div>
				<div class="back" style="margin: 0px 1px; height: 1px; border: 0px; line-height: 1px; $height: auto;">&nbsp;</div>
				<table class="back" style="width: 100%" cellpadding="0" cellspacing="0">
					<tr>
						<td style="width: 40px;" valign="top" align="right">
			<input id="hideLoginRadio" style="VERTICAL-ALIGN: middle" onclick="HideLoginRadioClick()"
				type="radio" runat="server" />
						</td>
						<td align="left">
			<label for="<%=hideLoginRadio.ClientID%>">Hide login field</label>
							<br /><br />
			<select class="wm_input" id="hideLoginSelect" runat="server">
				<option value="1" selected>Use Email as Login</option>
				<option value="0">Use Account-name as Login</option>
			</select>
							<br />
							<br />
						</td>
					</tr>
				</table>
				<div class= "back" style="margin: 0px 1px; height: 1px; border: 0px; line-height: 1px; $height: auto;">&nbsp;</div>
				<div class= "back" style="margin: 0px 2px; height: 1px; border: 0px; line-height: 1px; $height: auto;">&nbsp;</div>
			</div>
		
			<div id="hideLoginDiv3">
				<div class="back" style="margin: 0px 2px; height: 1px; border: 0px; line-height: 1px; $height: auto;">&nbsp;</div>
				<div class="back" style="margin: 0px 1px; height: 1px; border: 0px; line-height: 1px; $height: auto;">&nbsp;</div>
				<table class="back" style="width: 100%" cellpadding="0" cellspacing="0">
					<tr>
						<td style="width: 40px;" valign="top" align="right">
				<input id="hideEmailRadio" style="VERTICAL-ALIGN: middle" onclick="HideEmailRadioClick()"
					type="radio" runat="server">
						</td>
						<td valign="top" align="left">
			<label for="<%=hideEmailRadio.ClientID%>">Hide email field</label>
							<br /><br />
			<input type="text" name="txtUseDomain" id="txtUseDomain" class="wm_input" size="20" runat="server" />&nbsp;&nbsp;domain to use
							<br /><br />
			<input type="checkbox" name="intDisplayDomainAfterLoginField" value="1" id="intDisplayDomainAfterLoginField" class="wm_checkbox" runat="server" />
			&nbsp;<label for="<%=intDisplayDomainAfterLoginField.ClientID%>">Display domain after login field</label>
							<br /><br />
			<input type="checkbox" name="intLoginAsConcatination" id="intLoginAsConcatination" value="1" class="wm_checkbox" runat="server" />
			&nbsp;<label for="<%=intLoginAsConcatination.ClientID%>">Login as concatenation of "Login" field + "@" + domain</label>
							<br />
							<br />
						</td>
					</tr>
				</table>
				<div class= "back" style="margin: 0px 1px; height: 1px; border: 0px; line-height: 1px; $height: auto;">&nbsp;</div>
				<div class= "back" style="margin: 0px 2px; height: 1px; border: 0px; line-height: 1px; $height: auto;">&nbsp;</div>
			</div>
				
			<div style="padding-top: 10px;">
				<table style="width: 100%" cellpadding="0" cellspacing="0">
					<tr>
						<td style="width: 40px;" align="right">
		<input id="intAllowLangOnLogin" type="checkbox" runat="server" />
						</td>
						<td align="left">
		<label for="<%=intAllowLangOnLogin.ClientID%>">Allow choosing language on login</label>
						</td>
					</tr>
				</table>
			</div>
			
            <div style="padding-top: 10px;">
                <table cellspacing="0" cellpadding="0" style="width: 100%;">
                <tbody>
                    <tr>
                        <td style="width: 45px;"/>
                        <td class="wm_safety_info" style="padding: 0px;"> If enabled, it will be possible to select language on login page. </td>
                    </tr>
                </tbody>
                </table>
            </div>
			<br />
			
			<div style="padding-top: 10px;">
				<table style="width: 100%" cellpadding="0" cellspacing="0">
					<tr>
						<td style="width: 40px;" align="right">
		<input id="intAllowAdvancedLogin" type="checkbox" runat="server" />
						</td>
						<td align="left">
		<label for="<%=intAllowAdvancedLogin.ClientID%>">Allow advanced login</label>
						</td>
					</tr>
				</table>
			</div>
			
			<div style="padding-top: 10px;">
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td style="width: 45px;"></td>
						<td class="wm_safety_info" style="padding: 0px;">
							Allows changing SMTP and POP3/IMAP servers addresses,
							port numbers, enabling/disabling SMTP authentication from login panel.
						</td>
					</tr>
				</table>
			</div>
			<br />
			<div style="padding-top: 10px;">
				<table style="width: 100%" cellpadding="0" cellspacing="0">
					<tr>
						<td style="width: 40px;" align="right">
		<input id="intAutomaticCorrectLogin" type="checkbox" runat="server">
						</td>
						<td align="left">
		<label for="<%=intAutomaticCorrectLogin.ClientID%>">Automatically detect and 
					correct if user inputs e-mail instead of account-name</label>
						</td>
					</tr>
				</table>
			</div>
			
			<div style="padding-top: 10px;">
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td style="width: 45px;"></td>
						<td class="wm_safety_info" style="padding: 0px;">
							If a user typed a full e-mail address instead of just an account name during logging in, it'll be automatically
							corrected. Makes sense only with Standard login panel and Hide email field modes.
						</td>
					</tr>
				</table>
			</div>
			<br />
			<div>
				<hr size="1">
			</div>
			
			<div style="padding: 10px 0px; text-align: right">
		        <asp:Button id="SaveButton" runat="server" text="Save" Width="100" cssclass="wm_button" onclick="SaveButton_Click" />
			</div>
		</td>
	</tr>
</table>
<script type="text/javascript">
	var ID_PREFIX = "<%=clientPrefix%>_";
	wmLoginSettings.Init();
</script>