<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EndOfInstallation.ascx.cs" Inherits="EndOfInstallation" %>

<table class="wm_admin_center" width="550">
    <tr><td colspan="2"><br/></td></tr>
    <tr>
		<td colspan="2">
		    <span style="font-size: 14px">Step <%=_web_step%> of <%=_max_step%>:</span><br /><span style="font-size: 18px">Installation Completed</span>
		</td>
	</tr>
    <tr>
        <td colspan="2">
            <div class="wm_install_last_div_ok">
                Congratulations! You have successfully installed AfterLogic WebMail Lite .NET.
            <br/><br/>
                Click Exit to be redirected into the Admin Panel where you can set up
            <br/>
                domains and users.
            </div>
        </td>
    </tr>
	<tr><td colspan="2"><br /></td></tr>
	<tr><td colspan="2"></td></tr>
    <tr><td colspan="2"><hr size="1"/></td></tr>
	<tr>
        <td align="left">
            <input class="wm_install_button" type="button" onclick="javascript:document.location='install.aspx?mode=connection'" style="width: 100px;" value="Back" name="back_btn"/>
        </td>        
		<td align="right">
			<input type="submit" name="submit_btn" value="Exit" class="wm_install_button" />
		</td>
	</tr>
	<tr><td colspan="2"><br /><br /></td></tr>
</table>
