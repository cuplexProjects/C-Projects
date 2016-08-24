<%@ Page Title="" Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" CodeFile="ManageUser.aspx.cs" Inherits="ManageUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="CreateUserPanel" Visible="false" CssClass="createUserMainPanel" runat="server">
        <div>
            <span class="createUserHeader">Skapa ny användare</span>
        </div>
        <asp:Panel ID="CreateUserErrorMessagePanel" CssClass="loginErrorMessagePanel" Visible="false" runat="server"></asp:Panel>
        <div>
            <label for="UserNameTextBox" runat="server"><span>Användarnamn</span></label>
            <asp:TextBox CssClass="manageUserTextBox" ID="UserNameTextBox" MaxLength="50" autocomplete="off" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="UserNameTextBox" Text="!" CssClass="fieldValidator" runat="server"></asp:RequiredFieldValidator>
        </div>
        <div>
            <label id="Label1" for="EmailAddressTextBox" runat="server"><span>Epostadress</span></label>
            <asp:TextBox CssClass="manageUserTextBox" ID="EmailAddressTextBox" MaxLength="500" autocomplete="off" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="EmailAddressTextBox" Text="!" CssClass="fieldValidator" runat="server"></asp:RequiredFieldValidator>
        </div>
        <div>
            <label id="Label2" for="PasswordTextBox" runat="server"><span>Lösenord</span></label>
            <asp:TextBox CssClass="manageUserTextBox" ID="PasswordTextBox" TextMode="Password" autocomplete="off" MaxLength="50" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="PasswordTextBox" Text="!" CssClass="fieldValidator" runat="server"></asp:RequiredFieldValidator>
        </div>
        <div>
            <label id="Label3" for="PasswordConfirmTextBox" runat="server"><span>Bekräfta lösenord</span></label>
            <asp:TextBox CssClass="manageUserTextBox" ID="PasswordConfirmTextBox" TextMode="Password" autocomplete="off" MaxLength="50" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="PasswordConfirmTextBox" Text="!" CssClass="fieldValidator" runat="server"></asp:RequiredFieldValidator>
        </div>
        
        <div>
            <asp:Button ID="CreateUserButton" CssClass="createUserButton" OnClick="CreateUserButton_Clicked" Text="Skapa användare" runat="server" />
        </div>

    </asp:Panel>
    
    <asp:Panel ID="LoginUserPanel" Visible="false" CssClass="createUserMainPanel" DefaultButton="UserPanelLoginButton" runat="server">
        <div><span class="createUserHeader">Logga in</span></div>
        <asp:Panel ID="LoginErrorMessagePanel" CssClass="loginErrorMessagePanel" Visible="false" runat="server"></asp:Panel>        
            <div>
                <div>
                    <label id="Label4" for="UserName2TextBox" runat="server"><span>Användarnamn</span></label>
                    <asp:TextBox CssClass="manageUserTextBox" ID="UserName2TextBox" MaxLength="50" autocomplete="off" runat="server"></asp:TextBox>                    
                </div>
                <div>
                    <label id="Label5" for="Password2TextBox" runat="server"><span>Lösenord</span></label>
                    <asp:TextBox CssClass="manageUserTextBox" ID="Password2TextBox" MaxLength="50" TextMode="Password" autocomplete="off" runat="server"></asp:TextBox>
                </div>        
            </div>

            <div class="resetPasswordPanel">
                <label for="ResetPasswordEmail"><span>Ange epostadress för att återställa lösenord</span></label>
                <asp:TextBox ID="ResetPasswordEmail" Width="250px" runat="server"></asp:TextBox>
                <asp:Button ID="ResetPasswordButton" style="float:right;margin-top:10px;margin-right:20px;" Text="Återställ lösenord" OnClick="ResetPasswordButton_Clicked" runat="server" />
            </div>        
        <div><asp:Button ID="UserPanelLoginButton" OnClick="UserPanelLoginButton_Clicked" Text="Logga in" runat="server" /></div>
    </asp:Panel>
</asp:Content>

