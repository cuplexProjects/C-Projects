<%@ Control Language="C#" AutoEventWireup="true" Inherits="PlugIns_Security_security_settings" Codebehind="security-settings.ascx.cs" %>

<script type="text/javascript">
function ValidateForm()
{
    var txtPassword = document.getElementById("<%=txtSecurityPassword.ClientID%>");
    var txtPasswordConfirm = document.getElementById("<%=txtSecurityPasswordConfirm.ClientID%>");
    
    if (txtPassword.value != txtPasswordConfirm.value)
    {
        Tip.Show('Password confirmation field is not correct.', txtPasswordConfirm, '');
        return false;
    }
}
</script>

<div class="wm_contacts" id="main_contacts">
<table class="wm_settings" style="border-top: 0px;">
    <tr>
        <td class="wm_settings_cont">
            <table class="wm_settings_common" width="500">
            <tbody>
				<tr>
					<td colspan="2" class="wm_safety_info">
						You can change main admin (mailadm) password here. 
					</td>
				</tr>
                <tr><td colspan="2"><br /></td></tr>
                <tr>
                    <td class="wm_settings_list_select" colspan="2"><b>Change Password</b></td>
                </tr>
                <tr><td colspan="2" /></tr>
                <tr><td colspan="2" /></tr>
                <tr><td colspan="2" /></tr>
                <tr>
                    <td align="right">User Name:</td>
                    <td><b><%=UserName%></b></td>
                </tr>    
                <tr>
                    <td align="right">Password:</td>
                    <td>
				                            <asp:TextBox id="txtSecurityPassword" 
				                                         TextMode="Password"
				                                         TabIndex="2"
				                                         CssClass="wm_input" 
					                                     runat="server"
					                                     MaxLength="100" />                    
                    </td>
                </tr>    
                <tr>
                    <td align="right">Confirm password:</td>
                    <td>
				                            <asp:TextBox id="txtSecurityPasswordConfirm" 
				                                         TextMode="Password"
				                                         TabIndex="2"
				                                         CssClass="wm_input" 
					                                     runat="server"
					                                     MaxLength="100" />                    
                    </td>
                </tr>    
                <tr id="trHost" runat="server">
                    <td align="right">Host:</td>
                    <td><input type="text" runat="server" id="txtHost" class="wm_input" /></td>
                </tr>    
                <tr id="trPort" runat="server">
                    <td align="right">Port:</td>
                    <td><input type="text" runat="server" id="txtPort" class="wm_input" /></td>
                </tr>
                <tr><td colspan="2"><hr size="1"/></td></tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button id="SaveButton" style="FONT-WEIGHT: bold" runat="server" text="Save" Width="100" OnClientClick="javascript: return ValidateForm();" onclick="SaveSettings" />                
                    </td>    
                </tr>
            </tbody>
            </table>        
        </td>
    </tr>
</table>
</div>
<!--div id="lowtoolbar" class="wm_lowtoolbar">
    <span class="wm_lowtoolbar_messages"></span>
</div-->