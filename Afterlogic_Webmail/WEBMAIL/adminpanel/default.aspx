<%@ Page Language="C#" AutoEventWireup="true" Inherits="DefaultPage" EnableEventValidation="false" ValidateRequest="false" Codebehind="default.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="Expires" content="0" />
	<meta http-equiv="Cache-Control" content="no-cache" />
	<meta http-equiv="Pragma" content="no-cache" />
	<meta http-equiv="Content-Script-Type" content="text/javascript" />
	<title>Administration - </title>
	<link rel="stylesheet" href="styles/styles.css" type="text/css" />

	<script language="javascript" type="text/javascript" src="js/errorShow.js"></script>
	<script language="javascript" type="text/javascript" src="js/tooltip.js"></script>
	<script language="javascript" type="text/javascript" src="js/check-symbols.js"></script>
	<script language="javascript" type="text/javascript" src="js/common.js"></script>
	<script language="javascript" type="text/javascript" src="js/reports.js"></script>
	<script language="javascript" type="text/javascript" src="js/functions.js"></script>
	<script language="javascript" type="text/javascript" src="js/user.js"></script>
    <script language="javascript" type="text/javascript" src="js/install.js"></script>
    <script language="javascript" type="text/javascript">
    	var Browser = new CBrowser();
    	var _apPath = '';
    </script>
</head>

<body<%=Body%>>
<form id="form" runat="server">
	<asp:HiddenField runat="server" ID="errorMessageHiddenFieldID" Value="" />
	<asp:HiddenField runat="server" ID="errorMessage" Value="" />
	<asp:HiddenField runat="server" ID="reportMessage" Value="" />
    <asp:PlaceHolder id="mainPlaceHolder" runat="server"></asp:PlaceHolder>
</form>
</body>

<script type="text/javascript">
	Tip = new CTip();

    var errorMessage = document.getElementById("errorMessage");
    var reportMessage = document.getElementById("reportMessage");
    if(errorMessage.value != "")
    {
        MsgBox.Show(errorMessage.value, 2);
        errorMessage.value = "";
    }	
    if(reportMessage.value != "")
    {
        MsgBox.Show(reportMessage.value, 1);
        reportMessage.value = "";
    }
</script>
</html>
