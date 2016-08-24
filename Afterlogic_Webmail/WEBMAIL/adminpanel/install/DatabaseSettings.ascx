<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatabaseSettings.ascx.cs" Inherits="DatabaseSettings" %>

<script type="text/javascript">
    var ID_PREFIX = "<%=controlPrefix%>_";
</script>

<table class="wm_admin_center" width="550">
    <tr>
        <td colspan="2"><br/></td>
    </tr>
    <tr>
		<td colspan="2">
		    <span style="font-size: 14px">Step <%=_web_step%> of <%=_max_step%>:</span><br /><span style="font-size: 18px">Specify Database Settings</span>
		</td>
	</tr>
	<tr>
		<td colspan="2"><br/></td>
	</tr>
	<tr>
		<td colspan="2" align="left">
			<table>
				<tr>
					<td valign="top">
						<b>1.</b>
					</td>
					<td valign="top">
						<b>Select database engine to use</b>
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
				<tr>
				    <td colspan="2"><br /></td>
				</tr>
				<tr>
					<td valign="top">
						<b>2.</b>
					</td>
					<td valign="top">
						<b>Enter connection settings</b>
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
									<asp:textbox id="txtSqlPassword" runat="server" TextMode="password" Columns="45" CssClass="wm_install_input" />
								</td>
							</tr>
							<tr>
								<td align="right">
									<span id="txtSqlName_label" runat="server">Database&nbsp;name:</span>
								</td>
								<td>
									<input type="text" class="wm_install_input" id="txtSqlName" value="" size="32" runat="server"/>
                                    <input id="create_database" class="wm_install_test" type="button" value="Create" runat="server" onserverclick="create_database_Click"/>
                                    <%=_errorMessageCreateDB%>
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
									<input type="text" class="wm_install_input" id="txtAccessFile" size="45" runat="server" />
								</td>
							</tr>
							<tr>
							    <td colspan="2"><br /></td>
							</tr>
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
				<tr>
				    <td colspan="2"><br /></td>
				</tr>
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
                                    <input id="test_btn" class="wm_install_test" type="button" value="Test database" runat="server" onserverclick="test_connection_Click"/>
                                </td>
                                <td valign="middle"><%=_errorMessageConnection%></td>
                            </tr>
                        </tbody>
                        </table>                        
                    </td>
				</tr>
				<tr>
				    <td colspan="2"><br /></td>
				</tr>
				<tr>
					<td valign="top">
						<b>4.</b>
					</td>
					<td valign="top">
						<b>Specify prefix for table names (optional)</b>
						<br /><br />
						For instance, if you specify prefix as "my_", awm_accounts table will be created as "my_awm_accounts". You can leave it empty. 
						<br /><br />
						<input type="text" class="wm_install_input" id="DbPrefix" value="my_123" size="15" runat="server"/>
					</td>
				</tr>
				<tr>
				    <td colspan="2"><br /></td>
				</tr>
				<tr>
					<td valign="top">
						<b>5.</b>
					</td>
					<td valign="top">
						<b>Create Database Tables</b><br /><br />If enabled, this installer will create tables required by WebMail Lite. Disable it if you've already created the tables.
						<br /><br />
						<input type="checkbox" class="wm_checkbox" id="chNotCreate" value="1" checked="checked" runat="server" />
						<label for="<%=chNotCreate.ClientID%>">Create Database Tables</label>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td align="left" colspan="2"><br /></td>
	</tr>
	<tr>
		<td align="center" colspan="2"><%=_errorMessage%></td>
	</tr>
    <tr>
        <td align="center" colspan="2">
            <br/>
            Click Next to create the required database tables and proceed.
        </td>
    </tr>
	<!-- hr -->
	<tr>
		<td colspan="2">
			<hr size="1"/>
		</td>
	</tr>
	<tr>
        <td align="left">
            <input class="wm_install_button" type="button" onclick="javascript:document.location='install.aspx?mode=licensekey'" style="width: 100px;" value="Back" name="back_btn"/>
        </td>        
		<td align="right" valign="top">
            <input id="submit_btn" type="button" value="Next" class="wm_install_button" runat="server"  onserverclick="SubmitButton_Click"/>
		</td>
	</tr>
	<tr>
	    <td colspan="2"><br /><br /></td>
	</tr>
</table>
<script type="text/javascript">
	SettingsObjects["db"].Init();
</script>
