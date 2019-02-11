<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckSettings.ascx.cs" Inherits="CheckSettings" %>

<table class="wm_admin_center" width="550">
    <tr>
        <td><br/></td>
    </tr>
	<tr>
		<td>
			<span style="font-size: 14px">Step <%=_web_step%> of <%=_max_step%>:</span><br /><span style="font-size: 18px">Server Compatibility Test and Pre-Installation Check</span>
		</td>
	</tr>
	<tr><td><br /></td></tr>
    <tr>
        <td>The installer will now check that all the required server software is configured properly. It will also check if WebMail data folder is specified correctly.</td>
    </tr>
    <tr>
        <td><br/></td>
    </tr>
	<tr>
		<td>
            <table width="540">
                <tr>
                    <td style="padding:0px;" bgcolor="#f6f6f6">
                        <table width="100%" style="" cellpadding="0" cellspacing="0" class="wm_install_check_table">
	                        <tr>
		                        <td width="180" valign="top"><b>Sessions:</b></td>
		                        <td><%=_Sessions%><%=_SessionsMessage%></td>
	                        </tr>
	                     </table>
	                 </td>
	            </tr>
                <tr>
                    <td style="padding:0px;">
                        <table width="100%" style="<%=_DataFolderLocationStyle%>" cellpadding="0" cellspacing="0" class="wm_install_check_table">
	                        <tr>
		                        <td width="180" valign="top"><b>WebMail&nbsp;data&nbsp;folder:</b></td>
		                        <td><%=_DataFolderLocation%><%=_DataFolderLocationMessage%></td>
	                        </tr>
	                        <tr id="tr_CreatingDeletingFolders" runat="server">
		                        <td valign="top">&nbsp;&nbsp;&nbsp;&nbsp;Creating/deleting&nbsp;folders</td>
		                        <td><%=_CreatingDeletingFolders%><%=_CreatingDeletingFoldersMessage%></td>
	                        </tr>
	                        <tr id="tr_CreatingDeletingFiles" runat="server">
		                        <td valign="top">&nbsp;&nbsp;&nbsp;&nbsp;Creating/deleting&nbsp;files</td>
		                        <td><%=_CreatingDeletingFiles%><%=_CreatingDeletingFilesMessage %></td>
	                        </tr>
                        </table>
                    </td>
	            </tr>
                <tr>
                    <td style="padding:0px;" bgcolor="#f6f6f6">
                        <table width="100%" style="<%=_AdminPanelSettingsFileLocationStyle%>" cellpadding="0" cellspacing="0" class="wm_install_check_table">
	                        <tr id="tr_AdminPanelSettingsFileLocation" runat="server">
		                        <td width="180" valign="top"><b>Admin Panel Settings File:</b></td>
		                        <td><%=_AdminPanelSettingsFileLocation%><%=_AdminPanelSettingsFileLocationMessage%></td>
	                        </tr>
	                        <tr id="tr_AdminPanelReadSettingsFile" runat="server">
		                        <td valign="top">&nbsp;&nbsp;&nbsp;&nbsp;Read/write&nbsp;settings&nbsp;file</td>
		                        <td><%=_AdminPanelReadSettingsFile%> / <%=_AdminPanelWriteSettingsFile%><%=_AdminPanelReadWriteSettingsFileMessage%></td>
	                        </tr>
                        </table>
                    </td>
	            </tr>
                <tr>
                    <td style="padding:0px;">
                        <table width="100%" style="<%=_WebMailSettingsFileLocationStyle%>" cellpadding="0" cellspacing="0" class="wm_install_check_table">
	                        <tr id="tr_WebMailSettingsFileLocation" runat="server">
		                        <td width="180" valign="top"><b>WebMail Settings File:</b></td>
		                        <td><%=_WebMailSettingsFileLocation%><%=_WebMailSettingsFileLocationMessage%></td>
	                        </tr>
	                        <tr id="tr_WebMailReadSettingsFile" runat="server">
		                        <td valign="top">&nbsp;&nbsp;&nbsp;&nbsp;Read/write&nbsp;settings&nbsp;file</td>
		                        <td><%=_WebMailReadSettingsFile%> / <%=_WebMailWriteSettingsFile%><%=_WebMailReadWriteSettingsFileMessage%></td>
	                        </tr>
                        </table>
                    </td>
	            </tr>
                <tr>
                    <td style="padding:0px;" bgcolor="#f6f6f6">
                        <table width="100%" style="" cellpadding="0" cellspacing="0" class="wm_install_check_table">
	                        <tr>
		                        <td width="180" valign="top"><b>Sockets:</b></td>
		                        <td><%=_Sockets%><%=_SocketsMessage%></td>
	                        </tr>
	                     </table>
	                 </td>
	            </tr>            
                <tr>
                    <td style="padding:0px;">
                        <table width="100%" style="" cellpadding="0" cellspacing="0" class="wm_install_check_table">
	                        <tr>
		                        <td width="180" valign="top"><b>ODBC:</b></td>
		                        <td><%=_ODBC%><%=_ODBCMessage%></td>
	                        </tr>
	                     </table>
	                 </td>
	            </tr>            
            </table>
            <div id="_divResult" runat="server" class="wm_install_last_div_error"><%=_ResultMessage%></div>
        </td>
    </tr>    
	<tr><td><br /></td></tr>
	<tr><td><hr size="1" /></td></tr>
	<tr>
		<td align="right">
			<input type="button" value="<%=btnCheckName %>" class="wm_install_button" id="btnCheck" 
			onclick="<%=btnCheckOnClick%>" />
		</td>
	</tr>
    <tr><td><br/><br/></td></tr>
</table>