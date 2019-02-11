<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommonSettings.ascx.cs" Inherits="CommonSettings" %>

<script type="text/javascript">
function ValidateForm()
{
    var txtPassword = document.getElementById("<%=txtPassword.ClientID%>");
    var txtPasswordConfirm = document.getElementById("<%=txtPasswordConfirm.ClientID%>");
    
    if (txtPassword.value != txtPasswordConfirm.value)
    {
        Tip.Show('Password confirmation field is not correct.', txtPasswordConfirm, '');
        return false;
    }
}
</script>

<table class="wm_admin_center" width="550">
    <tr>
        <td colspan="2">
            <br/>
        </td>
    </tr>
	<tr>
		<td colspan="2">
			<span style="font-size: 14px">Step <%=_web_step%> of <%=_max_step%>:</span><br /><span style="font-size: 18px">Set Admin Panel Password</span>
		</td>
	</tr>
	<tr><td colspan="2"><br /></td></tr>
	<tr>
	    <td colspan="2">
	        To restrict access to the admin panel, it's strongly recommended to set a complex password. 
	    </td>
	</tr>
	<tr><td colspan="2"><br /></td></tr>
    <tr>
        <td width="200" align="right">Username:</td>
        <td width="350"><b><%=UserName%></b></td>
    </tr>    
    <tr>
        <td align="right">Password:</td>
        <td>
            <asp:TextBox id="txtPassword" TextMode="Password" TabIndex="2" CssClass="wm_install_input" runat="server" MaxLength="100" />                    
        </td>
    </tr>    
    <tr>
        <td align="right">Confirm password:</td>
        <td>
            <asp:TextBox id="txtPasswordConfirm" TextMode="Password" TabIndex="2" CssClass="wm_install_input" runat="server" MaxLength="100" />                    
        </td>
    </tr>    
    <tr><td colspan="2"><br/></td></tr>
    <tr><td colspan="2"></td></tr>
    <tr><td colspan="2"><hr size="1"/></td></tr>
    <tr>
        <td align="left">
            <input class="wm_install_button" type="button" onclick="javascript:document.location='install.aspx?mode=db'" style="width: 100px;" value="Back" name="back_btn"/>
        </td>        
        <td align="right">
            <asp:Button id="SaveButton" runat="server" text="Next" CssClass="wm_install_button" OnClientClick="javascript: return ValidateForm();" onclick="SaveSettings" />                
        </td>    
    </tr>
    <tr><td colspan="2"><br/><br/></td></tr>
</table>