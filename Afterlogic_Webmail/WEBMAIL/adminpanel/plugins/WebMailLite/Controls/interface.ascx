<%@ Control Language="C#" AutoEventWireup="true" Inherits="interface_settingsLite" Codebehind="interface.ascx.cs" %>

<table class="wm_settings_common" width="550" border="0">
	<tr>
		<td width="150"></td>
		<td></td>
	</tr>
	<tr>
		<td colspan="2" class="wm_settings_list_select"><b>Interface Settings</b></td>
	</tr>
	<tr>
		<td colspan="2"><br /></td>
	</tr>
	<tr>
		<td align="right">Mails per page:</td>
		<td>
			<input type="text" id="intMailsPerPage" runat="server" size="4" MaxLength="4" class="wm_input" />
			<asp:requiredfieldvalidator id="mailsReqValidator" Runat="server" Display="Dynamic" ErrorMessage="!" ControlToValidate="intMailsPerPage"></asp:requiredfieldvalidator>
			<asp:RangeValidator ID="mailsRangeValidator" Runat="server" EnableClientScript="True" ControlToValidate="intMailsPerPage" Display="Dynamic" ErrorMessage="!" Type="Integer" MinimumValue="0" MaximumValue="9999" ></asp:RangeValidator>
		</td>
	</tr>
    <tr>
		<td align="right">&nbsp;</td>
		<td>
			<input type="checkbox" value="1" id="intRightMessagePane" name="intRightMessagePane" class="wm_checkbox" runat="server" />
			&nbsp;<label for="<%=intRightMessagePane.ClientID%>">The message pane is to the right of the message list, rather than below</label>
		</td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td>
			<input type="checkbox" value="1" id="intAlwaysShowPictures" name="intAlwaysShowPictures" class="wm_checkbox" runat="server" />
			&nbsp;<label for="<%=intAlwaysShowPictures.ClientID%>">Always show pictures in messages</label>
		</td>
	</tr>	
	<tr>
		<td align="right">Default skin:</td>
		<td>
			<select id="txtDefaultSkin" runat="server" class="wm_input" style="width: 150px" />
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<input type="checkbox" runat="server" style="vertical-align: middle" id="intAllowUsersChangeSkin" />
			<label for="<%=intAllowUsersChangeSkin.ClientID%>"">Allow users to change skin</label>
		</td>
	</tr>
	<tr>
		<td align="right">Default language:</td>
		<td>
			<select runat="server" id="txtDefaultLanguage" class="wm_input" style="width: 150px">
			</select>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<input type="checkbox" runat="server" id="intAllowUsersChangeLanguage" style="vertical-align: middle" />
			<label for="<%=intAllowUsersChangeLanguage.ClientID%>">Allow users to change interface language</label>
		</td>
	</tr>
	<tr class="wm_hide">
		<td></td>
		<td>
			<input type="checkbox" runat="server" style="vertical-align: middle" id="intShowTextLabels" />
			<label for="<%=intShowTextLabels.ClientID%>">Show text labels</label>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<input type="checkbox" runat="server" style="vertical-align: middle" id="intAllowDHTMLEditor" />
			<label for="<%=intAllowDHTMLEditor.ClientID%>">Allow DHTML editor</label>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<input type="checkbox" runat="server" style="vertical-align: middle" id="intAllowContacts" />
			<label for="<%=intAllowContacts.ClientID%>">Allow Contacts</label>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<input type="checkbox" disabled="disabled" value="1" runat="server" style="vertical-align: middle" id="intAllowCalendar" />
            <font color="#aaaaaa"><label for="<%=intAllowCalendar.ClientID%>">Allow Calendar (Pro only)</label></font>
		</td>
	</tr>
    <!-- hr -->
	<tr>
		<td colspan="2"><hr size="1" /></td>
	</tr>
	<tr>
        <td></td>
		<td align="right">
			<asp:Button runat="server" id="SubmitButton" text="Save" cssclass="wm_button" Width="100" onclick="SubmitButton_Click" />
		</td>
	</tr>
</table>
