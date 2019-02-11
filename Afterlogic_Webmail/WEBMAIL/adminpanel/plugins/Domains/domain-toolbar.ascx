<%@ Control Language="C#" AutoEventWireup="true" Inherits="PlugIns_Domains_domain_toolbar" Codebehind="domain-toolbar.ascx.cs" %>
<tr>
	<td>
		<asp:PlaceHolder ID="NewDomain" Runat="server">
			<div runat="server" id="div_reg" class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="javascript:NewDomain('reg');">
				<img title="New Internal Domain" src="images/menu/new-domain.png" class="wm_menu_new_group_img" style="width:20px; height:14"/>
				<span class="">New Domain</span>
			</div>
			<div runat="server" id="div_vir" class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="javascript:NewDomain('vir');">
				<img title="New Forwarded Domain" src="images/menu/new-domain.png" class="wm_menu_new_group_img" style="width:20px; height:14"/>
				<span class="">New Forwarded Domain</span>
			</div>
			<div runat="server" id="div_rem" class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="javascript:NewDomain('rem');">
				<img title="New External Domain" src="images/menu/new-domain.png" class="wm_menu_new_group_img" style="width:20px; height:14"/>
				<span class=""><% if (_isServerExist) Response.Write("New External Domain"); else Response.Write("New Domain");%></span>
			</div>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="PlaceholderContacts" Runat="server">
			<div class="wm_toolbar_item" onmouseover="this.className='wm_toolbar_item_over'" onmouseout="this.className='wm_toolbar_item'" onclick="javascript:DoDelete();">
				<img alt="Delete selected domain(s)" title="Delete" src="images/menu/delete.gif" class="wm_menu_delete_img"/>
				<span class="">Delete</span>
			</div>
		</asp:PlaceHolder>
	</td>
</tr>