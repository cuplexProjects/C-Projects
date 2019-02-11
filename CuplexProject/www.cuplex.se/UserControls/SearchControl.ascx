<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchControl.ascx.cs" Inherits="UserControls_SearchControl" %>
<asp:Panel ID="SearchMainPanel" CssClass="searchControl dropShadow" DefaultButton="SearchButton" runat="server">    
    <asp:DropDownList ID="CategoryDropdownList" runat="server"></asp:DropDownList>
    <asp:TextBox ID="SearchTextBox" CssClass="SearchTextBox" Text="Sök efter länk" onkeyup="searchTextChanged=true;" onfocus="TextFieldActive(this);" onblur="TextFieldInActive(this);" MaxLength="50" runat="server"></asp:TextBox>
    <asp:Button ID="SearchButton" CssClass="searchButton" Text="Sök" OnClientClick="return canSearch();" OnClick="SearchButton_Clicked" runat="server" />
</asp:Panel>