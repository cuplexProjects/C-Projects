<%@ Control Language="C#" AutoEventWireup="true" Inherits="PlugIns_Security_main" Codebehind="main.ascx.cs" %>
<div class="wm_settings">
	<div class="wm_settings_row">
		<div class="wm_settings_cont" style="height: auto;">
			<asp:PlaceHolder ID="ContentPlaceHolder" runat="server"></asp:PlaceHolder>
		</div>
		<div class="wm_settings_nav">
			<div style="width: 215px; height: 1px; overflow: hidden; padding: 0px;"></div>
			<div class="wm_selected_settings_item">
				<nobr><img src="images/dot.png"/> <a href="default.aspx?mode=auth">Authentication Settings</a></nobr>
			</div>
		</div>
		<div class="clear">
		</div>
	</div>
</div>
