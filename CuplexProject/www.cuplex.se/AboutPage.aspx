<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="AboutPage.aspx.cs" Inherits="AboutPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="AboutPanel" CssClass="aboutPanel" runat="server">
        <cuc:TextControl ID="AboutHeader" Text="Om Cuplex.se" ResourceKey="AboutLabel" CssClass="headerLabel" runat="server"></cuc:TextControl>
        <cuc:TextControl ID="AboutText" Text="." ResourceKey="AboutPageText" CssClass="aboutPageText" runat="server"></cuc:TextControl>
        
    </asp:Panel>
</asp:Content>

