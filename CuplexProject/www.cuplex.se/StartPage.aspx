<%@ Page Title="" Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" CodeFile="StartPage.aspx.cs" Inherits="StartPage" %>
<%@ Register src="UserControls/ContentListBlock.ascx" tagname="ContentListBlock" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:ContentListBlock ID="ContentListBlockControl" runat="server" />
</asp:Content>