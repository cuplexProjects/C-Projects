<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LicenseKeySettings.ascx.cs" Inherits="LicenseKeySettings" %>

<table class="wm_admin_center" width="550">
    <tr>
        <td colspan="2">
            <br/>
        </td>
    </tr>
    <tr>
		<td colspan="2">
		    <span style="font-size: 14px">Step <%=_web_step%> of <%=_max_step%>:</span><br /><span style="font-size: 18px">Enter The License Key</span>
		</td>
	</tr>
	<tr><td colspan="2"><br /></td></tr>
	<tr>
		<td width="100" align="right">
			 License key:
		</td>
		<td>
			<input id="txtLicenseKey" type="text" class="wm_input" style="width: 350px" value="" runat="server"/>
		</td>
	</tr>
	
	<tr>
		<td></td>
		<td>
			<font color="red"><%=_errorMessage%></font><br/>
		<%if (_EmptyKey == null)
        { %>
            If you do not have a license key, you can get 30-day trial key <asp:HyperLink runat="server" NavigateUrl="http://www.afterlogic.com/download/get-trial-key?productid=17" id="GetLicenseUrl" Text="here" Target=_blank/>.	
		<%} %>
        </td>
	</tr>
	<tr><td colspan="2"><br /></td></tr>
	<tr><td colspan="2"></td></tr>
	<tr><td colspan="2"><hr size="1" /></td></tr>
	<tr>
        <td align="left">
            <input class="wm_install_button" type="button" onclick="javascript:document.location='install.aspx?mode=license'" style="width: 100px;" value="Back" name="back_btn"/>
        </td>        
		<td align="right">
			<input type="button" name="submit_btn" value="Next" class="wm_install_button" runat="server"  onserverclick="SubmitButton_Click"/>
		</td>
	</tr>
    <tr><td colspan="2"><br/><br/></td></tr>
</table>