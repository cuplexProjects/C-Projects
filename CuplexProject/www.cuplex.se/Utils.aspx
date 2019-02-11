<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Utils.aspx.cs" Inherits="UtilsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:Panel ID="LoggedInContent" runat="server">
    <asp:Panel ID="CheckIpPanel" CssClass="utilsPanel" runat="server">
        <asp:Label ID="IpNumberLabel" Text="IP number:" runat="server"></asp:Label>
        <asp:TextBox ID="IpNumberTextBox" MaxLength="15" runat="server"></asp:TextBox>
        
        <asp:Button ID="CheckIpButton" OnClick="CheckIpButton_Clicked" Text="IP Lookup" runat="server" /><br />
        <asp:Label ID="IpLookupResultLabel" Text="&nbsp;" runat="server"></asp:Label>    
    </asp:Panel>
    
    <asp:Panel ID="ResolveHostNamePanel"  CssClass="utilsPanel" runat="server">
        <asp:Label ID="HostName" Text="Host Name:" runat="server"></asp:Label>
        <asp:TextBox ID="HostNameTextBox" MaxLength="100" runat="server"></asp:TextBox>
        <asp:Button ID="HostLookupButton" OnClick="HostLookupButton_Clicked" Text="DNS lookup" runat="server" /><br />
        <asp:Label ID="HostNameLookupResult" Text="&nbsp;" runat="server"></asp:Label>
    </asp:Panel>
    
    <asp:Panel ID="RandomPasswordPanel" runat="server">
        <asp:Label ID="RandomPasswordDescriptionLabel" Text="Create a random password" runat="server"></asp:Label><br />
        <asp:TextBox ID="LengthOfPasswordTextBox" MaxLength="3" Text="32" Width="50px" runat="server"></asp:TextBox>
        <asp:TextBox ID="PasswordOutputTextBox" ReadOnly="true" Width="300px" runat="server"></asp:TextBox><br />
        <asp:Button ID="CreatePasswordButton" OnClick="CreatePasswordButton_Clicked" Text="CreatePassword" runat="server" /><br />
        
    </asp:Panel>
</asp:Panel>

<asp:Panel ID="NotLoggedInContent" runat="server">
    <div>Du måste vara inloggad för att få tillgång till denna sidan.</div>
</asp:Panel>

</asp:Content>

