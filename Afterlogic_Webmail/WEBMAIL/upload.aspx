<%@ Page language="c#" Codebehind="upload.aspx.cs" AutoEventWireup="False" Inherits="WebMail.upload" validateRequest="false" %>
<% if (!flashUpload) { %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html>
<head>
	<title></title>
</head>
<body>
	<% if (errorOccured) { %>
	<script type="text/javascript">
		alert('<%=error%>');
	</script>
	<% } else { %>
	<script type="text/javascript">
		parent.LoadAttachmentHandler({FileName: '<%=name%>', TempName: '<%=tmp_name%>', Size: <%=size%>, MimeType: '<%=mime_type%>', Inline: <%=inlineImage%>, Url: '<%=url%>'});
	</script>
	<% } %>
</body>
</html>

<% } else {
		if (errorOccured) { %>
attachment = {Error: '<%=error%>'};
	<%  } else { %>
attachment = {	FileName: '<%=name%>',
				TempName: '<%=tmp_name%>',
				Size: <%=size%>,
				MimeType: '<%=mime_type%>',
				Inline: <%=inlineImage%>,
				Url: '<%=url%>',
				Error: ''};
	<%	}
	} %>
