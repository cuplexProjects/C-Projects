<%@ Page language="c#" Codebehind="check-mail.aspx.cs" AutoEventWireup="True" Inherits="WebMail.check_mail" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<HTML>
	<body onload="parent.CheckEndCheckMailHandler();">
		<script type="text/javascript">
		<%
		Response.Write("parent.EndCheckMailHandler('" + errorDesc + "');");
		%>
		</script>
		<%Response.Flush();%>
	</body>
</HTML>
