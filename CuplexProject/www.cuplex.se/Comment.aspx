<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Comment.aspx.cs" Inherits="CommentPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kommentarer</title>
    <link type="text/css" href="css/general.css" rel="Stylesheet" />
    <style type="text/css">html{overflow:hidden!important;} </style>
    <script type="text/javascript" src="script/jquery-1.4.2.min.js"></script>
</head>
<body class="backWhite">
    <script type="text/javascript">
        $(document).ready(function() {
            $("#CommentTextBox").addClass("CommentTextBoxIdle");
            $("#CommentTextBox").focus(function() {
                $(this).addClass("CommentTextBox").removeClass("CommentTextBoxIdle");
            }).blur(function() {
                $(this).removeClass("CommentTextBox").addClass("CommentTextBoxIdle");
            });
        });
    </script>

    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="CommentPanel" CssClass="commentPanel dropShadow" runat="server">
            
        </asp:Panel>
        
        <asp:Panel ID="PostCommentPanel" CssClass="postCommentPanel dropShadow" Visible="false" runat="server">
            <asp:TextBox ID="CommentTextBox" CssClass="" TextMode="MultiLine" Rows="5" Width="350" runat="server"></asp:TextBox>
            <asp:Button ID="PostCommentButton" CssClass="PostCommentButton" OnClick="PostCommentButton_Clicked" Text="Kommentera" runat="server" />
        </asp:Panel>
    </div>
    </form>
</body>
</html>