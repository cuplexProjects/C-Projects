<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Bookmarks.aspx.cs" Inherits="Bookmarks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <cuc:TextControl ID="BookmarksDescriptionText" ResourceKey="BookmarksDescriptionText" runat="server"></cuc:TextControl>

    <asp:Panel ID="NotLoggedInPanel" runat="server">
        <cuc:TextControl ID="NotLoggedInText" ResourceKey="NotLoggedInText" runat="server"></cuc:TextControl>
    </asp:Panel>

    <asp:Panel ID="LoggedInPanel" runat="server">

    <asp:FileUpload ID="FileUploadControl"  runat="server" />
    <asp:Button ID="UploadButton" OnClick="UploadButton_Clicked" runat="server" />

    </asp:Panel>


    <asp:RegularExpressionValidator 
     id="RegularExpressionValidator1" runat="server" 
     ErrorMessage="" 
     ValidationExpression="([a-zA-Z\\-]|[0-9])+.json$"
     ControlToValidate="FileUploadControl"></asp:RegularExpressionValidator>


</asp:Content>

