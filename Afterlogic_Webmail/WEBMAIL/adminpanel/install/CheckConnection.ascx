<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckConnection.ascx.cs" Inherits="CheckConnection" %>

<table class="wm_admin_center" width="550">
	<tr><td colspan="2"><br /></td></tr>

	<tr>
		<td colspan="2">
			<span style="font-size: 14px">Step <%=_web_step%> of <%=_max_step%>:</span>
			<br />
			<span style="font-size: 18px">Check Connection with E-mail Server</span>
		</td>
	</tr>
	<tr><td colspan="2"><br /></td></tr>
    <tr>
        <td colspan="2"> Here you can test the connectivity with your e-mail server (optional). </td>
    </tr>
	<tr><td colspan="2"><br /></td></tr>
	<tr>
	    <td colspan="2">
	        <table>
	            <tr>
	                <td>E-mail server host:</td>
		            <td>
			            <input type="text" id="txtHost" size="25" class="wm_install_input" value="localhost" runat="server" />
		            </td>
	            </tr>	
	            <tr>
	                <td></td>
		            <td>
			            <input type="checkbox" class="wm_checkbox" id="chSMTP" value="1"  checked="checked" runat="server"/>&nbsp;<label for="<%=chSMTP.ClientID%>" class="">SMTP</label>
			            &nbsp;
			            <input type="checkbox" class="wm_checkbox" id="chPOP3" value="1"  checked="checked" runat="server"/>&nbsp;<label for="<%=chPOP3.ClientID%>" class="">POP3</label>
			            &nbsp;
			            <input type="checkbox" class="wm_checkbox" id="chIMAP4" value="1"  checked="checked" runat="server"/>&nbsp;<label for="<%=chIMAP4.ClientID%>" class="">IMAP4</label>
		            </td>
	            </tr>	
	            <tr>
	                <td></td>
	                <td>
    		            <input id="test_btn" class="wm_install_test" type="submit" value="Test connection" runat="server" onserverclick="TestConnectionButton_Click"/>
	                </td>
	            </tr>
	        </table>
	    </td>
	</tr>
	<tr>
		<td colspan="2" align="left"><%=_connectionResult%><br/><%=_errorMessage%></td>
	</tr>

	<tr><td colspan="2" align="center"></td></tr>
    <tr><td colspan="2"></td></tr>
	<tr><td colspan="2"><hr size="1" /></td></tr>
	<tr>
		<td align="left">
			<input type="button" name="back_btn" value="Back" class="wm_install_button" style="width: 100px" onclick="javascript:document.location='install.aspx?mode=common'" />
		</td>
		<td align="right">
        
        <input type="submit" id="submit_btn1" value="Next" class="wm_install_button" style="width: 100px" runat="server" onserverclick="NextButton_Click" onclick="" />
		</td>
	</tr>
	<tr><td colspan="2"><br /><br /></td></tr>
</table>
<script type="text/javascript">
if (window.SettingsObjects && SettingsObjects["socket"]) {
	SettingsObjects["socket"].Init();
}
</script>

