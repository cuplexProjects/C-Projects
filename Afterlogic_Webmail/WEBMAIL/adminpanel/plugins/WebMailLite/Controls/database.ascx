<%@ Control Language="C#" AutoEventWireup="true" Inherits="database_settingsLite" Codebehind="database.ascx.cs" %>

<!-- script -->
<script type="text/javascript">
    var ID_PREFIX = "<%=controlPrefix%>_";
</script>

<table class="wm_settings_common" width="550">
	<tr>
		<td width="170"></td>
		<td></td>
	</tr><!-- 1 -->
	<tr>
		<td class="wm_settings_list_select" colspan="2"><b>Database Settings</b></td>
	</tr>
	<tr>
		<td colspan="2"><br/>
		</td>
	</tr>
	<tr>
		<td colspan="2" align="left">
			<table>
				<tr>

					<td valign="top">
						<b>1.</b>
					</td>
					<td valign="top">
						<b>Database engine</b>
						<br /><br />
						<asp:RadioButton CssClass="wm_checkbox" GroupName="intDbType" id="intDbTypeMsSql" value="1" runat="server" 
						/>&nbsp;<label id="intDbTypeMsSql_label" runat="server"><font style="font-size: 12px">MS&nbsp;SQL</font></label>
						<br />
						<asp:RadioButton CssClass="wm_checkbox" GroupName="intDbType" id="intDbTypeMySql" value="3" runat="server" 
						/>&nbsp;<label id="intDbTypeMySql_label" runat="server"><font style="font-size: 12px">MySQL</font></label>
						<br />
						<asp:RadioButton CssClass="wm_checkbox" GroupName="intDbType" id="intDbTypeMsAccess" value="2" runat="server" 
						/>&nbsp;<label id="intDbTypeMsAccess_label" runat="server"><font style="font-size: 12px">MS&nbsp;Access</font></label>
					</td>
				</tr>
				<tr><td colspan="2"><br /></td></tr>
				<tr>
					<td valign="top">
						<b>2.</b>
					</td>
					<td valign="top">
						<b>Connection settings</b>
						<br /><br />
						<table id="dbSwitcher">
							<tr>
								<td align="right">
									<span id="txtSqlLogin_label" runat="server">SQL&nbsp;login:</span>

								</td>
								<td>
									<input type="text" class="wm_install_input" id="txtSqlLogin" value="" size="45" runat="server"/>
								</td>
							</tr>
							<tr>
								<td align="right">
									<span id="txtSqlPassword_label" runat="server">SQL&nbsp;password:</span>

								</td>
								<td>
									<asp:textbox id="txtSqlPassword" runat="server" TextMode="password" Columns="45" CssClass="wm_input" />
								</td>
							</tr>
							<tr>
								<td align="right">
									<span id="txtSqlName_label" runat="server">Database&nbsp;name:</span>
								</td>
								<td>
									<input type="text" class="wm_install_input" id="txtSqlName" value="" size="45" runat="server"/>
                                    <input id="create_database" class="wm_hide" type="button" value="Create" />
								</td>
							</tr>		
							<tr>
								<td align="right">
									<span id="txtSqlSrc_label" runat="server">Host:</span>
								</td>
								<td>
									<input type="text" class="wm_install_input" id="txtSqlSrc" value="" size="45" runat="server" />
								</td>
							</tr>
							<tr><td colspan="2"><div id="dbMessageDiv"></div></td></tr>
							<tr>
								<td align="right">
									<span id="txtAccessFile_label" runat="server">MS Access file (*.mdb):</span>
								</td>
								<td>
									<input type="text" class="wm_input" id="txtAccessFile" size="45" runat="server" />
								</td>
							</tr>
							<tr><td colspan="2"><br /></td></tr>
							<tr>
								<td align="right">
									<input type="checkbox" value="1" class="wm_checkbox" id="useDSN" runat="server" /><span id="useDSN_label" runat="server"><label for="<%=useDSN.ClientID%>">&nbsp;ODBC Data source&nbsp;(DSN):</label></span>
								 </td>
								<td>
									<input type="text" class="wm_install_input" id="txtSqlDsn" value="" size="45" runat="server" />
								</td>
							</tr>		
							<tr>
								<td align="right" nowrap>
									<input type="checkbox" value="1" class="wm_checkbox" id="useCS" runat="server" /><span id="odbcConnectionString_label" runat="server"><label for="<%=useCS.ClientID%>" id="useCS_label">&nbsp;<font style="font-size: 12px">ODBC&nbsp;Connection&nbsp;String:&nbsp;</font></label></span>
								</td>
								<td>
									<input type="text" class="wm_install_input" id="odbcConnectionString" value="" size="45" runat="server" />
								</td>
							</tr>
						</table>						
					</td>
				</tr>
				<tr><td colspan="2"><br /></td></tr>
				<tr>
					<td valign="top">
						<b>3.</b>
					</td>
					<td valign="top">
						<b>Test database connectivity to check if the specified settings are correct (recommended)</b>
						<br/>
                        <br/>
                        <table cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td valign="top">
                                    <input id="test_btn" class="wm_install_test" type="button" value="Test connection" runat="server" onserverclick="test_connection_Click"/>
                                </td>
                                <td valign="top">
                                    <asp:Button ID="create_tables" CssClass="wm_install_test" Text="Create tables" Runat="server" OnClick="create_tables_Click" OnClientClick="return CheckAccess();" />
                                </td>
                                <td valign="top">
                                    <input type="button" id="Button1" class="wm_install_test" value="Update" onclick="window.open('plugins/webmaillite/updatedb.aspx');" />
                                </td>
                                <td valign="middle"></td>
                            </tr>
                        </tbody>
                        </table>                        
                    </td>
				</tr>
			</table>
		</td>
	</tr>
	<!-- hr -->
	<tr>
		<td colspan="2">
			<hr size="1" />
		</td>
	</tr>
	<tr>
		<td colspan="2" align="right" valign="top">
            <input id="submit_btn" size="100" style="width: 100px; font-weight: bold;" class="wm_button" type="button" value="Save" runat="server" onserverclick="submit_btn_Click"/>
		</td>
	</tr>
</table>

<!-- startup script -->
<script type="text/javascript">
	function CheckAccess()
	{
        if (!$('<%=intDbTypeMsAccess.ClientID%>').checked) return true; 
        else
        { 
            alert('Warning: You can create only MySQL or MS SQL Server database tables!'); 
            return false; 
        }	
    }
	SettingsObjects["db"].Init();
</script>

