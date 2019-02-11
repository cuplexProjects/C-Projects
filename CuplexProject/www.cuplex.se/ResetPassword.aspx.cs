using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CLinq = CuplexLib.Linq;
using CuplexLib.LinqExtensions;

public partial class ResetPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected override void  OnPreRender(EventArgs e)
    {
        string resetIdentifyer = Request.QueryString["resetId"];
        if (resetIdentifyer != null && resetIdentifyer.Length == 64)
        {
            using (var db = CLinq.DataContext.Create())
            {
                var passwordReset = db.PasswordResets.Where(pr => pr.ResetIdentyfier == resetIdentifyer).Take(1).SingleOrDefault();
                if (passwordReset == null)
                {
                    NotAuthenticatedPanel.Visible = true;
                    Label errorMessage = new Label();
                    errorMessage.Text = "Angivet id är felaktigt!";
                    NotAuthenticatedPanel.Controls.Add(errorMessage);
                }
                else
                {
                    CuplexLib.User user = CuplexLib.User.GetOne(passwordReset.UserRef);
                    AccessControl.CreateUserSession(user.UserName);
                    db.PasswordResets.DeleteBatch(pr => pr.UserRef == user.UserRef);
                    AuthenticatedPanel.Visible = true;
                }
            }
        }
    }
}
