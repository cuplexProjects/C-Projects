<%@ Page Language="C#" AutoEventWireup="true" Inherits="PlugIns_WebMailLite_updatedb" Codebehind="updatedb.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<HTML>
	<HEAD>
		<title></title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
	    <meta http-equiv="Expires" content="0" />
	    <meta http-equiv="Cache-Control" content="no-cache" />
	    <meta http-equiv="Pragma" content="no-cache" />
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<p runat="server" ID="P1" NAME="P1"><%=sb.ToString()%></p>
		<form id="Form1" method="post" runat="server">
			<hr />
			<asp:Label ID="outputLabel" Runat="server" Font-Size="Large"></asp:Label>
			<br />
		</form>
	</body>
</HTML>
