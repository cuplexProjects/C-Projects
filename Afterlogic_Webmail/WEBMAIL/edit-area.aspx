<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit-area.aspx.cs" Inherits="WebMail.edit_area" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html<%=_rtl%>>
<head>
    <style> .misspel { background: url(skins/redline.gif) repeat-x bottom; } </style>
	<script>
		z = 0;
		for (var i=0; i<10000; i++) {
			z = z + i*89/52 - i*56/78;
		}
	</script>
</head>
<body onload="parent.EditAreaLoadHandler();">
</body>
</html>
