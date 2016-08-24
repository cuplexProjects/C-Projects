<%@ Page language="c#" Codebehind="history-storage.aspx.cs" AutoEventWireup="True" Inherits="WebMail.history_storage" validateRequest="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html>
<body>
<script>
<% if (historyKey.Length > 0) { %>
parent.<% Response.Write(objectName); %>.ProcessHistory('<% Response.Write(historyKey); %>');
<% } %>
</script>
</body>
</html>