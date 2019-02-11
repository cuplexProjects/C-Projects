using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuplexLib;

public partial class ManageUser : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //HttpContext.Current.RewritePath("/user/login");

        string script = @"
            function VerifyPassword()
            {
                if (getObj('" + PasswordTextBox.ClientID + @"').value != getObj('" + PasswordConfirmTextBox.ClientID + @"').value) {
                    alert('Lösenorden matchar inte!');
                    return false;
                }
                else
                    return true;
            }
        ";

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "CreateUserScripts", script, true);
    }
    protected override void OnPreRender(EventArgs e)
    {
        Label errorLabel = new Label();
        if (Session["LastError"] is string)        
            errorLabel.Text = Session["LastError"].ToString();

        string action = this.Request.QueryString["action"];
        if (action == "createuser")
        {
            CreateUserPanel.Visible = true;
            this.Title = "Skapa ny användare";

            if (!string.IsNullOrEmpty(errorLabel.Text))
            {
                CreateUserErrorMessagePanel.Controls.Add(errorLabel);
                CreateUserErrorMessagePanel.Visible = true;
            }
        }
        else if (action == "login")
        {
            LoginUserPanel.Visible = true;
            this.Title = "Logga in";

            if (!string.IsNullOrEmpty(errorLabel.Text))
            {
                LoginErrorMessagePanel.Controls.Add(errorLabel);
                LoginErrorMessagePanel.Visible = true;
            }
        }
        base.OnPreRender(e);
    }
    
    protected void CreateUserButton_Clicked(object sender, EventArgs e)
    {
        string userName = UserNameTextBox.Text;
        string password = PasswordTextBox.Text;
        string emailAddress = EmailAddressTextBox.Text;
        
        if (userName == "" || password == "" || emailAddress == "")
            return;

        //Check if user exists
        if (CuplexLib.User.UserNameExists(userName))
        {
            ShowModalMessage(Utils.GetResourceText("RegisterUserUsernameTaken"));            
            return;            
        }
        else if (!Utils.ValidateEmail(emailAddress))
        {
            ShowModalMessage(Utils.GetResourceText("RegisterUserIncorrectEmailAddress"));
            return;
        }
        else if (password != PasswordConfirmTextBox.Text)
        {
            ShowModalMessage(Utils.GetResourceText("RegisterUserPasswordMissmatch"));            
            return;
        }

        //Verify password strength
        PasswordVerifier passwordVerifier = new PasswordVerifier();
        PasswordVerifier.PasswordVerifierResult passwordVerificationResult = passwordVerifier.VerifyPassword(password);

        if (passwordVerificationResult != PasswordVerifier.PasswordVerifierResult.PasswordIsOk)
        {
            string errorMessage = "<b>" + Utils.GetResourceText(Enum.GetName(typeof(PasswordVerifier.PasswordVerifierResult), passwordVerificationResult)) + "</b>";
            errorMessage += "<BR/>" + string.Format(Utils.GetResourceText("PasswordRules"), passwordVerifier.MinimumPasswordLength, passwordVerifier.MinimumPasswordCapitalLetters, passwordVerifier.MinimumPasswordDigits);
            ShowModalMessage(errorMessage);
            return;
        }

        //Verify UserName
        if (!UserNameVerifier.VerifyUserName(userName))
        {
            ShowModalMessage(Utils.GetResourceText("IncorrectUserName"));
            return;
        }

        CuplexLib.User createdUser = CuplexLib.User.CreateUser(userName, password, emailAddress);
        if (createdUser != null)
        {
            AccessControl.CreateUserSession(createdUser.UserName);
            Response.Redirect(cms.Current.GetRootPath);
        }
    }
    protected void UserPanelLoginButton_Clicked(object sender, EventArgs e)
    {
        string userName = UserName2TextBox.Text;
        string password = Password2TextBox.Text;

        User.UserAuthenticateResponce authenticationResponce = CuplexLib.User.AuthenticateUser(userName, password);
        if (authenticationResponce == CuplexLib.User.UserAuthenticateResponce.UnknownUser)
        {
            Session["LastError"] = "Användaren hittades inte.";
            //Response.Redirect(cms.Current.GetRootPath + "user/createuser");
        }
        else if (authenticationResponce == CuplexLib.User.UserAuthenticateResponce.PasswordIncorrect)
        {
            Session["LastError"] = "Felaktigt lösenord.";
            //Response.Redirect(cms.Current.GetRootPath + "user/login");
        }
        else
        {
            AccessControl.CreateUserSession(userName);
            Response.Redirect(cms.Current.GetRootPath);
        }
    }
    protected void ResetPasswordButton_Clicked(object sender, EventArgs e)
    {
        if (PasswordReset.SendPasswordResetEmail(ResetPasswordEmail.Text))
        {
            Session["LastError"] = "Ett epostmeddelande har skickats";
            ResetPasswordEmail.Text = "";
        }
    }
}
