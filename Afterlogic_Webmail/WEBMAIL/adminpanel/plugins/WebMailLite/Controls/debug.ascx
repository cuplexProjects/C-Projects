<%@ Control Language="C#" AutoEventWireup="true" Inherits="debug_settingsLite" Codebehind="debug.ascx.cs" %>
<!-- [start center] -->
<script type="text/javascript">
	function PopUpWindow(url)
	{
		var shown = window.open(url, 'Popup',
			'left=(screen.width-700)/2,top=(screen.height-400)/2,'+
			'toolbar=no,location=no,directories=no,status=yes,scrollbars=yes,resizable=yes,'+
			'copyhistory=no,width=700,height=400');
		shown.focus();
		return false;
	}
</script>
<table class="wm_settings_common" width="550" border="0">
	<tbody>
		<tr> <td width="60"></td>		<td></td></tr>
		<tr>
			<td class="wm_settings_list_select" colspan="2"><b>Debug</b></td>
		</tr>
		<tr>
			<td colspan="2"><br/></td>
		</tr>
		<tr>
			<td align="right"><input type="checkbox" id="intEnableLogging" runat="server"/></td>
			<td><label for="<%=intEnableLogging.ClientID%>">Enable debug logging</label></td>
		</tr>
            <tr>
		        <td align="right">
		        </td>
                <td>
	                <div class="wm_safety_info">
		                Enables detailed logging helpful for troubleshooting.
	                </div>
                </td>
            </tr>		            
		<tr>
			<td align="right"></td>
			<td align="left">&nbsp;Path for log&nbsp;&nbsp;<input type="text" id="txtPathForLog" class="wm_input" runat="server" style="WIDTH: 330px"
					readonly/></td>
		</tr>
		<tr>
			<td></td>
			<td align="left">
				<input type="button" runat="server" id="ShowAllLogButton" value="View Entire Log" class="wm_install_test"
					style="FONT-SIZE: 11px" onclick="PopUpWindow('plugins/webmail/show-log.aspx');"/>&nbsp;&nbsp;
				<input type="button" runat="server" id="ShowPartialLogButton" Value="View Log (last records)"
					class="wm_install_test" style="FONT-SIZE: 11px" onclick="PopUpWindow('plugins/webmail/show-log.aspx?mode=part');"/>&nbsp;&nbsp;
				<input type="button" runat="server" value="Clear Log" id="ClearLogButton" class="wm_install_test"
					style="FONT-SIZE: 11px" onserverclick="ClearLogButton_ServerClick"/>
			</td>
		</tr>
        <!-- hr -->
		<tr>
			<td colspan="2">
				<hr size="1"/>
			</td>
		</tr>
		<tr>
	        <td></td>
			<td align="right">
				<asp:Button id="SaveButton" text="Save" cssclass="wm_button" Runat="server" Width="100" onclick="SaveButton_Click" />
			</td>
		</tr>
	</tbody>
</table>
