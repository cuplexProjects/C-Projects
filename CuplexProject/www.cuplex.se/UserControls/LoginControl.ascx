<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoginControl.ascx.cs" Inherits="UserControls_LoginControl" %>
<asp:Panel ID="NotLoggedInMainWrapper" DefaultButton="LoginButton" runat="server">
    <div class="loginControl dropShadow">    
        <div class="loginText"><span>Logga in</span></div>
        <asp:TextBox ID="UserNameTextBox" CssClass="loginTextField" MaxLength="50" onfocus="TextFieldActive(this);" onblur="TextFieldInActive(this)" runat="server">Användarnamn</asp:TextBox>
        <asp:TextBox ID="PasswordTextBox" TextMode="Password" CssClass="loginTextField" MaxLength="50" onfocus="TextFieldActive(this);" onblur="TextFieldInActive(this)" runat="server">Lösenord</asp:TextBox>
        <asp:Button ID="LoginButton" CssClass="loginButton" Text="Logga in" OnClick="LoginButton_Clicked" CausesValidation="false" runat="server" />    
    </div>
    <div class="loginControlSub dropShadow">
        <div><span>Välkomment till Cuplex.se här hittar du nya roliga länkar varje dag.</span></div>
        <div><span>Som inloggad användare kan du betygsätta, föreslå och kommentera länkar.</span></div>
        <div class="createAccountLink"><a id="createUserLink" href="" runat="server">Skapa nytt konto</a></div>
    </div>
</asp:Panel>

<asp:Panel ID="LoggedInMainWrapper" CssClass="userLoggedInControl dropShadow" Visible="false" runat="server">
    <div id="loggedInUserBox" class="loggedInUserLabel" runat="server"></div>
    <ul class="loggedInLinks">
        <li><a id="homeLink" href="/" runat="server">Startsidan</a></li>
        <li><a id="suggestionLink" href="/" runat="server">Föreslå länk</a></li>
        <li><a id="accountSettings" href="/" runat="server">Kontoinställningar</a></li> 
        <asp:PlaceHolder ID="AdminLinkPlaceholder" Visible="false" runat="server"><li><a id="adminLink" href="/" runat="server">Administration</a></li> </asp:PlaceHolder>
        <li><a id="logoutLink" href="/" runat="server">Logga ut</a></li>
    </ul>
    
</asp:Panel>
