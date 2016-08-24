<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="SuggestLink.aspx.cs" Inherits="SuggestLink" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="SuggestLinkPanel" CssClass="createUserMainPanel" runat="server">
        <div><span class="createUserHeader">Föreslå länk</span></div>
        <asp:Panel ID="LinkSuggestMessagePanel" runat="server" Visible="false"></asp:Panel>
        <div>
            <label id="Label1" for="DescriptionTextBox" runat="server"><span>Beskrivning</span></label>
            <asp:TextBox CssClass="manageUserTextBox" ID="DescriptionTextBox" MaxLength="250" autocomplete="off" runat="server"></asp:TextBox>
        </div>
        <div>
            <label id="Label2" for="UrlTextBox" runat="server"><span>Url</span></label>
            <asp:TextBox CssClass="manageUserTextBox" ID="UrlTextBox" MaxLength="500" autocomplete="off" runat="server"></asp:TextBox>
        </div>
        
        <div>
            <label id="Label3" for="PasswordTextBox" runat="server"><span>Kategori</span></label>
            <asp:DropDownList ID="CategoryDropdownList" runat="server"></asp:DropDownList>
        </div>
        <div><asp:Button ID="SuggestLinkButton" Text="Föreslå" OnClick="SuggestLinkButton_Clicked" runat="server" /></div>
    </asp:Panel>
</asp:Content>

