<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="css/bootstrap.min.css" />

    <!-- Optional theme -->
    <link rel="stylesheet" href="css/bootstrap-theme.min.css" />

    <link rel="stylesheet" href="css/general.css" />

    <title>Admin</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>        
        <asp:Panel ID="AdminPanel" CssClass="main_panel" runat="server">
            <div class="form-group">
                <div class="form-group"><h1>Welcome</h1></div>
            
                <div class="form-group">
                    <label for="FilePathTextBox">File path:</label>                    
                    <asp:TextBox ID="FilePathTextBox" CssClass="form-control" placeholder="File path" runat="server"></asp:TextBox>
                    <div class="checkbox">
                        <asp:CheckBox ID="IncludeSubdirsCheckbox" Text="Inclunde subfolders when entering a full path" runat="server" />
                    </div>
                </div>

                <div class="form-group">
                    <label for="FilePathTextBox">url:</label>
                    <asp:TextBox ID="urlTextBox" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Button ID="GenerateButton" CssClass="btn btn-primary" style="float:right" Text="generate" OnClick="GenerateButton_click" runat="server" />
                </div>
                <div>
                    <asp:Label ID="ErrorLabel" CssClass="label label-warning" runat="server"></asp:Label>
                </div>
            </div>
        </asp:Panel>
    </div>
    </form>

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script type="text/javascript" src="https://code.jquery.com/jquery.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script type="text/javascript" src="js/bootstrap.min.js"></script>
</body>
</html>
