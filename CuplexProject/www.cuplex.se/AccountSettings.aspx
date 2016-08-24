<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="AccountSettings.aspx.cs" Inherits="AccountSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="AccountSettingsPanel" CssClass="createUserMainPanel" runat="server">
        <div><span class="createUserHeader">Redigera användare</span></div>
        <div>
            <label id="Label1" for="EmailAddressTextBox" runat="server"><span>Epostadress</span></label>
            <asp:TextBox CssClass="manageUserTextBox" ID="EmailAddressTextBox" MaxLength="100" autocomplete="off" runat="server"></asp:TextBox>
        </div>
        <div>
            <label id="Label2" for="PasswordTextBox" runat="server"><span>Lösenord</span></label>
            <asp:TextBox CssClass="manageUserTextBox" ID="PasswordTextBox" MaxLength="50" TextMode="Password" autocomplete="off" runat="server"></asp:TextBox>
        </div>
        <div>
            <label id="Label3" for="PasswordConfirmTextBox" runat="server"><span>Bekräfta lösenord</span></label>
            <asp:TextBox CssClass="manageUserTextBox" ID="PasswordConfirmTextBox" MaxLength="50" TextMode="Password" autocomplete="off" runat="server"></asp:TextBox>
        </div>
        
        <div><asp:CheckBox ID="UpdatePasswordCheckBox" CssClass="checkbox" Text="Uppdatera lösenord" runat="server" /></div>
        
        <div><asp:Button ID="UpdateUserSettingsButton" Text="Uppdatera användare" OnClientClick="return confirmPassword();" OnClick="UpdateUserSettingsButton_Clicked" runat="server" /></div>
    </asp:Panel>
</asp:Content>

