<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="SiteLinks.aspx.cs" Inherits="SiteLinks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<cuc:TextControl ID="SiteLinksHeaderText" CssClass="siteLinksLabel" ResourceKey="SiteLinksHeaderText" runat="server"></cuc:TextControl>
<asp:Panel ID="SiteLinkPanel" CssClass="siteLinksPanel" runat="server"></asp:Panel>

<hr />
<cuc:TextControl ID="SiteLinksDescription" CssClass="siteLinksDescription" ResourceKey="SiteLinksDescription" runat="server"></cuc:TextControl>
</asp:Content>
