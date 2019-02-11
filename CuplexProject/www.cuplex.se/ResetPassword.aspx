<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="ResetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <asp:Panel ID="ResetPasswordPanel" CssClass="resetPasswordMainPanel" runat="server">
 
    <asp:Panel ID="AuthenticatedPanel" Visible="false" runat="server">    
        <a id="messageLink" href="AccountSettings.aspx" class="link" runat="server">För att byta lösenord behöver du gå till Kontoinställningar</a>
    </asp:Panel>
    
    <asp:Panel ID="NotAuthenticatedPanel" Visible="false" style="color:Red;" runat="server">
        
    </asp:Panel>
 </asp:Panel>
</asp:Content>

