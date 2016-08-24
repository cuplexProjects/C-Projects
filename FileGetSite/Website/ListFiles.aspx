<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ListFiles.aspx.cs" Inherits="ListFilesPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Download</title>
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="css/bootstrap.min.css" />

    <!-- Optional theme -->
    <link rel="stylesheet" href="css/bootstrap-theme.min.css" />

    <link rel="stylesheet" href="css/general.css" />
</head>
<body>    
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="ErrorMessageLabel" runat="server"></asp:Label>
        <asp:Panel ID="FileLinkPanel" CssClass="FileLinkPanel" runat="server">

        </asp:Panel>
    </div>
    </form>
</body>
</html>
