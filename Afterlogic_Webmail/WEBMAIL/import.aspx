<%@ Page language="c#" Codebehind="import.aspx.cs" AutoEventWireup="False" Inherits="WebMail.import" validateRequest="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html>
<head>
	<title></title>
</head>
<body>
<!--
get-parameters:
file_type - input radio
fileupload - input file
-->
<!--
execution code:
0 - error
1 - ok
2 - no contacts for import.
-->
	<script language="JavaScript" type="text/javascript">
		parent.ImportContactsHandler( <%=_jsErrorCode%>, <%=_jsContactsImported%>);
	</script>
</body>
</html>


