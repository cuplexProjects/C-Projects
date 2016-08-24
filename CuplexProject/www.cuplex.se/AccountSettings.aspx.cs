using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuplexLib;

public partial class AccountSettings : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["User"] == null)
                Response.Redirect(cms.Current.GetRootPath);
            else
            {
                CuplexLib.User user = (CuplexLib.User)Session["User"];
                EmailAddressTextBox.Text = user.EmailAddress;
            }
        }

        string script = @"
        function confirmPassword() {
            var password1 = document.getElementById('" + PasswordTextBox.ClientID + @"').value;
            var password2 = document.getElementById('" + PasswordConfirmTextBox.ClientID + @"').value;
            var updatePassword =document.getElementById('" + UpdatePasswordCheckBox.ClientID + @"').checked;  
            
            if (!updatePassword) return true;

            if (password1 != password2) {
                alert('Lösenorden matchar inte!');
                return false;
            }            
            else if (password1.length < 5){
                alert('Lösenordet är inte tillräckligt långt!');
                return false;
            }
            return true;
        }";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "AccountSettingsScript", script, true);
    }
    protected bool ValidateBeforeSave()
    {
        if (UpdatePasswordCheckBox.Checked)
        {
            if (PasswordTextBox.Text != PasswordConfirmTextBox.Text)
                return false;
            else if (PasswordTextBox.Text.Length < 5)
                return false;
        }
        
        if (!Utils.ValidateEmail(EmailAddressTextBox.Text))
        {
            base.ShowModalMessage("Epostadressen är ogiltlig");
            return false;
        }
       
        return true;
    }
    protected void UpdateUserSettingsButton_Clicked(object sender, EventArgs e)
    {
        CuplexLib.User user = Session["User"] as CuplexLib.User;
        string password = null;

        if (UpdatePasswordCheckBox.Checked)
            password = PasswordTextBox.Text;

        if (user == null || !ValidateBeforeSave())
            return;

        Session["User"] = CuplexLib.User.UpdateUser(user.UserRef, password, Utils.TruncateString(EmailAddressTextBox.Text, 100));
    }
}
