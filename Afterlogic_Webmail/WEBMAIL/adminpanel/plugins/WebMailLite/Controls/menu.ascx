<%@ Control Language="C#" AutoEventWireup="true" Inherits="menuLite" Codebehind="menu.ascx.cs" %>

<div style="width: 215px; height: 1px; overflow: hidden; padding: 0px;"></div>
<div id="divCommon" runat="server">
	<nobr><img src="images/dot.png"/> <a href="?mode=webmail">Common Settings</a></nobr>
</div>
<div id="divInterface" runat="server">
	<nobr><img src="images/dot.png"/> <a href="?mode=interface">Interface Settings</a></nobr>
</div>
<div id="divLogin" runat="server">
	<nobr><img src="images/dot.png"/> <a href="?mode=login">Login Settings</a></nobr>
</div>
<div id="divDatabase" runat="server">
	<nobr><img src="images/dot.png"/> <a href="?mode=db">Database Settings</a></nobr>
</div>
<div id="divDebug" runat="server">
	<nobr><img src="images/dot.png"/> <a href="?mode=debug">Debug & Logging</a></nobr>
</div>
<div id="divServer" runat="server">
	<nobr><img src="images/dot.png"/> <a href="?mode=integration">Server Integration</a></nobr>
</div>
