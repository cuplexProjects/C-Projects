using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuplexLib;

public partial class SettingsPage :BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CuplexLib.User user = Session["User"] as CuplexLib.User;

        if (user == null || !user.IsAdmin)
        {
            Response.Redirect(cms.Current.GetRootPath, true);
            return;
        }

        SaveButton.Text = Utils.GetResourceText("SaveButton");                
        RenderSettingsTable();
    }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }
    private void RenderSettingsTable()
    {
        using (CuplexLib.Linq.DataContext db = CuplexLib.Linq.DataContext.Create())
        {
            List<CuplexLib.Linq.Setting> settingsList = db.Settings.ToList();

            Table settingsTable = new Table();
            settingsTable.ID = "SettingsTable";
            settingsTable.CellPadding = 0;
            settingsTable.CellSpacing = 0;
            settingsTable.CssClass = "settingsTable";
            SettingsPanel.Controls.Add(settingsTable);

            TableHeaderRow headerRow = new TableHeaderRow();
            TableHeaderCell headerCell = new TableHeaderCell();

            headerCell.Text = "KeyType";
            headerRow.Cells.Add(headerCell);

            headerCell = new TableHeaderCell();
            headerCell.Text = "Description";
            headerRow.Cells.Add(headerCell);

            headerCell = new TableHeaderCell();
            headerCell.Text = "Value";
            headerRow.Cells.Add(headerCell);

            settingsTable.Rows.Add(headerRow);

            foreach (var setting in settingsList)
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();

                cell.CssClass = "settTblCellFixedData label";
                cell.Text = setting.KeyType;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.CssClass = "settTblCellFixedData";
                cell.Text = setting.Description;
                row.Cells.Add(cell);

                cell = new TableCell();

                if (setting.DataType.HasValue && setting.DataType.Value == SettingsDataType.Boolean)
                {
                    CheckBox settingsCheckbox = new CheckBox();
                    settingsCheckbox.CssClass = "settTblChkBox";
                    settingsCheckbox.Attributes.Add("KeyType", setting.KeyType);
                    if (!IsPostBack)
                        settingsCheckbox.Checked = setting.Value == "1";
                    cell.Controls.Add(settingsCheckbox);
                }
                else
                {
                    TextBox settingsValueTextBox = new TextBox();
                    settingsValueTextBox.Style.Add(HtmlTextWriterStyle.Width, "96%");
                    settingsValueTextBox.Attributes.Add("KeyType", setting.KeyType);
                    if (!IsPostBack)
                        settingsValueTextBox.Text = setting.Value;
                    cell.Controls.Add(settingsValueTextBox);
                }

                row.Cells.Add(cell);
                settingsTable.Rows.Add(row);
            }
        }
    }
    protected void SaveButton_Clicked(object sender, EventArgs e)
    {
        Table settingsTable = GetPageControlById(this.Page, "SettingsTable") as Table;
        if (settingsTable == null)
            return;

        using (CuplexLib.Linq.DataContext db = CuplexLib.Linq.DataContext.Create())
        {
            List<Control> textBoxList = GetPageControlsByType(settingsTable, typeof(TextBox));
            List<Control> checkBoxList = GetPageControlsByType(settingsTable, typeof(CheckBox));
            Dictionary<string, CuplexLib.Linq.Setting> dbSettingsDictionary = db.Settings.ToDictionary(s => s.KeyType);
            List<string> KeyTypeList = new List<string>();

            foreach (Control ctrl in textBoxList)
            {
                TextBox settingsTextBox = ctrl as TextBox;
                if (settingsTextBox != null && settingsTextBox.Attributes["KeyType"] != null)
                {
                    string keyType = settingsTextBox.Attributes["KeyType"];
                    if (dbSettingsDictionary.ContainsKey(keyType))
                    {
                        //Verify that the correct data type is used
                        if (Settings.IsCorrectDataType(settingsTextBox.Text, dbSettingsDictionary[keyType].DataType))
                        {
                            dbSettingsDictionary[keyType].Value = settingsTextBox.Text;
                            KeyTypeList.Add(keyType);
                        }
                        else
                            settingsTextBox.Text = dbSettingsDictionary[keyType].Value;
                    }
                }
            }
            foreach (Control ctrl in checkBoxList)
            {
                CheckBox settingsCheckBox = ctrl as CheckBox;
                if (settingsCheckBox != null && settingsCheckBox.Attributes["KeyType"] != null)
                {
                    string keyType = settingsCheckBox.Attributes["KeyType"];
                    if (dbSettingsDictionary.ContainsKey(keyType))
                    {
                        //Verify that the correct data type is used
                        if (dbSettingsDictionary[keyType].DataType.HasValue && dbSettingsDictionary[keyType].DataType.Value==SettingsDataType.Boolean)
                        {
                            dbSettingsDictionary[keyType].Value = (settingsCheckBox.Checked ? "1" : "0");
                            KeyTypeList.Add(keyType);
                        }
                    }
                }
            }
            db.SubmitChanges();
            Settings.ClearCache(KeyTypeList);
        }
    }
}