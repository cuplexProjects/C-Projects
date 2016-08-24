using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : System.Web.UI.Page
{
    public BasePage()
    {

    }
    protected void ShowModalMessage(string message)
    {
        Panel modalBackgroundPanel = (Panel)this.Master.FindControl("ModalBackgroundPanel");
        Panel modalWindow = (Panel)this.Master.FindControl("ModalWindow");
        Panel modalMessagePanel = (Panel)this.Master.FindControl("ModalWindow");
        Button modalOkButton = (Button)this.Master.FindControl("ModalOkButton");
        modalOkButton.CausesValidation = false;

        if (modalBackgroundPanel != null)
            modalBackgroundPanel.Visible = true;
        if (modalWindow != null)
            modalWindow.Visible = true;

        if (modalMessagePanel != null)
        {
            Label messageLabel = new Label();
            messageLabel.Text = message;
            modalMessagePanel.Controls.Add(messageLabel);
        }
    }
    protected void ShowModalBackground()
    {
        Panel modalBackgroundPanel = (Panel)this.Master.FindControl("ModalBackgroundPanel");
        if (modalBackgroundPanel != null)
            modalBackgroundPanel.Visible = true;
    }
    protected void HideModalBackground()
    {
        Panel modalBackgroundPanel = (Panel)this.Master.FindControl("ModalBackgroundPanel");
        if (modalBackgroundPanel != null)
            modalBackgroundPanel.Visible = false;
    }
    protected Control GetPageControlById(Control rootControl, string controlId)
    {
        Control retCtrl = null;

        if (rootControl.ID != null && rootControl.ID == controlId)
            return rootControl;

        foreach (Control c in rootControl.Controls)
        {
            retCtrl = GetPageControlById(c, controlId);
            if (retCtrl != null)
                return retCtrl;
        }

        return retCtrl;
    }
    protected List<Control> GetPageControlsByType(Control root, Type controlType)
    {
        List<Control> controlList = new List<Control>();

        foreach (Control ctrl in root.Controls)
        {
            if (ctrl.GetType() == controlType)
                controlList.Add(ctrl);
            else
                controlList.AddRange(GetPageControlsByType(ctrl, controlType));
        }

        return controlList;
    }
}
