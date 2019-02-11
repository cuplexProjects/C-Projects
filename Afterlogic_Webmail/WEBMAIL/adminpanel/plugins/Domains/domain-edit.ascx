<%@ Control Language="C#" AutoEventWireup="true" Inherits="PlugIns_Domains_domain_edit" Codebehind="domain-edit.ascx.cs" %>

<script type="text/javascript">
var ID_SCREEN_PREFIX = "<%=ClientPrefix%>_";
function AddValueToList(value, list)
{
	if (value == '') return false;
	
	var newListItem = document.createElement('OPTION');

	newListItem.text = value;
	newListItem.value = value;
	
	list.options.add(newListItem);

	return true;
}

function DeleteSelectedFromList(list)
{
	if ((list.selectedIndex >= 0) && (list.selectedIndex < list.length))
	{
		list.remove(list.selectedIndex);
	}
}

function mailProtocolChange() 
{
	// change port
	var select = $(ID_SCREEN_PREFIX + 'intIncomingMailProtocol');
	var port = $(ID_SCREEN_PREFIX + 'intIncomingMailPort');
	port.value = (select.value == 'POP3') ? '110' : '143';
}

function customViewTab(tabNum)
{
	var i;
	for (i = 1; i <= 2; i++)
	{
		if(i == tabNum)
		{
			$('content_custom_tab_' + tabNum).style.display = 'block';
			$('custom_tab_' + tabNum).className = 'wm_settings_switcher_select_item';
		}
		else
		{
			$('content_custom_tab_' + i).style.display = 'none';
			$('custom_tab_' + i).className = 'wm_settings_switcher_item';
		}
	}
}

function IsDomainEntered()
{
	if ($(ID_SCREEN_PREFIX + 'textDomainName').value.trim() == '')
	{
		alert('Enter Domain Name');
		return false;
	}
//	SelectListAll($(ID_SCREEN_PREFIX + 'RedirectionsListDDL'));
	SelectListAll($(ID_SCREEN_PREFIX + 'LRedirectionsListDDL'));
	return true;
}

function SelectListAll(list)
{
    for (var i=0; i<list.options.length; i++)
        list.options[i].selected = true;
}
</script>
<asp:HiddenField ID="hidUid" Value="" runat="server"/>

<div style="display: none;">
	<asp:HiddenField ID="CUSTOM_TAB_NUM_ID" Value="" runat="server" />
</div>
<div style="float: right; margin: 10px;">
	<img src="images/close-popup.png" class="wm_close_icon" onclick="document.location = '?close'" title="Close" alt="X" />
</div>
<table border="0" cellpadding="0" cellspacing="0" width="95%" class="wm_contacts_view">
	<tbody>
	<tr>
	    <td colspan="2"><span style="font-size: large;"><%=_actionDomain%></span></td>
	</tr>    
	<tr>
	    <td colspan="2">
	        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <%if (!_isUpdate) {%>
                <tr><td colspan="3"><br /></td></tr>	            
                <tr>
		            <td></td>
		            <td align="left" colspan="2">
			            <asp:Literal ID="litDomainDescriptionView" runat="server"/>
		            </td>
	            </tr>	
                <tr><td colspan="3"><br /></td></tr>
                <%} %>	            
		        <tr>
			        <td align="right" width="100px">
				        Domain name:
			        </td>
			        <td>
        		        <div style="width:200px; overflow:hidden;" id="divDomainName" runat="server"><asp:Literal ID="litDomainName" runat="server"/></div>
				        <input id="textDomainName" type="text" class="wm_input" size="20" style="width: 140px"
					        runat="server" maxlength="100" />
			        </td>
		        </tr>
		        <tr>
			        <td align="right" width="100px">
			        </td>
			        <td>
				        <asp:Literal ID="litDomainDescription" runat="server"/>
			        </td>
		        </tr>
            </table>			        
         </td>   
        </tr>  
        
		<tr id="tr_custom_options" class="wm_hide" runat="server">
			<td colspan="2">
			    <input type="hidden" name="hidCustomDomain" id="hidCustomDomain" runat="server" />
				<div class="wm_settings_accounts_info" style="width: 100%; margin: 30px 0px" id="divDomainTabs" runat="server">
					<div class="wm_settings_switcher_indent">
					</div>
					<div id="custom_tab_2" class="wm_settings_switcher_item" onclick="customViewTab(2)">
						<a href="javascript:void(0)">Advanced Options</a>
					</div>
					<div id="custom_tab_1" class="wm_settings_switcher_select_item" onclick="customViewTab(1)">
						<a href="javascript:void(0)">Forwards</a>
					</div>
				</div>
				<table id="content_custom_tab_1" style="display: block;padding-left:30px;" class="hide" width="100%"
					border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td>Forwards</td>
						<!--td>Active Forwards</td-->
					</tr>
					<tr>
						<!--td>
							<input type="text" id="RedirectionID" tabindex="39" width="180" class="wm_input" maxlength="100" />
						</td-->
						<td>
							<input type="text" id="LRedirectionID" tabindex="43" width="180" class="wm_input" maxlength="100" />
						</td>
					</tr>
					<tr>
						<!--td>
							<input type="button" id="ButtonaddAddForward" class="wm_button" TabIndex="40" value="Add Forward" onclick="AddValueToList($('RedirectionID').value, $(ID_SCREEN_PREFIX + 'RedirectionsListDDL'));" />
						</td-->
						<td>
							<input type="button" id="ButtonAddActiveForward" class="wm_button" TabIndex="44" value="Add Forward" onclick="AddValueToList($('LRedirectionID').value, $(ID_SCREEN_PREFIX + 'LRedirectionsListDDL'));"/>
						</td>
					</tr>
					<tr>
						<td colspan="2">&nbsp;</td>
					</tr>
					<tr>
						<!--td>
							<asp:ListBox id="RedirectionsListDDL" Rows="6" tabindex="41" class="wm_input"
								style="width: 200px" runat="server"></asp:ListBox>
						</td-->
						<td>
							<asp:ListBox id="LRedirectionsListDDL" Rows="6" tabindex="45" class="wm_input"
								style="width: 200px" multiple="multiple" runat="server"></asp:ListBox>
						</td>
					</tr>
					<tr>
						<!--td>
							<input type="button" id="ButtonDeleteForward" class="wm_button" value="Delete Forward" tabindex="42" onclick="DeleteSelectedFromList($(ID_SCREEN_PREFIX + 'RedirectionsListDDL'));"/>
						</td-->
						<td>
							<input type="button" id="ButtonDeleteActiveForward" class="wm_button" tabindex="46" value="Delete Forward" onclick="DeleteSelectedFromList($(ID_SCREEN_PREFIX + 'LRedirectionsListDDL'));"/>
						</td>
					</tr>
				</table>
				<table id="content_custom_tab_2" style="display: none;padding-left:30px;" class="hide" width="100%" 
					border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td>
							Advanced Custom Domain Processing
						</td>
					</tr>
					<tr>
						<td>
							<asp:TextBox ID="AdvancedID" TabIndex="47" runat="server" TextMode="MultiLine" Columns="55"
								CssClass="wm_input" Rows="6" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr id="tr_webmail_options" class="wm_hide" runat="server">
			<td colspan="2">
				<input type="hidden" name="hidWebMailDomainID" id="hidWebMailDomainID" runat="server" />
				<table width="100%" border="0"  cellspacing="0" cellpadding="0">
					<tr>
						<td align="right" width="100px">
							Incoming mail:
						</td>
						<td>
							<input type="text" id="txtIncomingMail" maxlength="100" runat="server" class="wm_input">&nbsp;&nbsp;
							<nobr>Port:&nbsp;<input type="text" id="intIncomingMailPort" maxlength="4" size="3" runat="server" class="wm_input">
			&nbsp;<select id="intIncomingMailProtocol" runat="server" onchange="mailProtocolChange()" class="wm_input">
				<option value="POP3" selected>POP3</option>
				<option Value="IMAP4">IMAP4</option>
			</select></nobr>
						</td>
					</tr>
					<tr>
						<td align="right">
							Outgoing mail:
						</td>
						<td align="left" colspan="2">
							<input type="text" id="txtOutgoingMail" maxlength="100" runat="server" class="wm_input">&nbsp;&nbsp;
							Port:&nbsp;<input type="text" id="intOutgoingMailPort" maxlength="4" size="3" runat="server"
								class="wm_input">
						</td>
					</tr>
					<tr>
						<td align="right">&nbsp;</td>
						<td colspan="2">
							<input type="checkbox" id="intReqSmtpAuthentication" style="vertical-align: middle"
								runat="server">
							<label for='<%=intReqSmtpAuthentication.ClientID%>'>Requires SMTP authentication</label>
						</td>
					</tr>
	                
				</table>
			</td>
		</tr>

        <tr runat="server" id="trForHr">
	        <td colspan="2">
		        <hr size="1" />
	        </td>
        </tr>
        <tr>
	        <td align="right" colspan="2">
	            <asp:Button ID="ButtonSave" Width="100px" CssClass="wm_button" Text="Save " OnClientClick="return IsDomainEntered();" OnClick="SaveDomain_OnClick" runat="server" />&nbsp;
	        </td>
        </tr>
	</tbody>
</table>
