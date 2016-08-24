<%@ Page language="c#" Codebehind="session-saver.aspx.cs" AutoEventWireup="True" Inherits="WebMail.session_saver" validateRequest="false" %>
<%
Random randObj = new Random();
%><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html>
<head>
	<META HTTP-EQUIV="refresh" content="120;URL=session-saver.aspx?<% Response.Write(Server.UrlEncode(randObj.NextDouble().ToString())); %>">
</head>
<body>
</body>
</html>