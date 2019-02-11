<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="SettingsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="SettingsPanel" runat="server">        
    </asp:Panel>
    <br />
    <asp:Button ID="SaveButton" OnClick="SaveButton_Clicked" runat="server" />
</asp:Content>