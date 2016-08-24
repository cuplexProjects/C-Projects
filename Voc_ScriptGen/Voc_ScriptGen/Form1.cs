using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Voc_ScriptGen
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> descriptionDictionary = null;
        public Form1()
        {
            InitializeComponent();
            toolTipHelp.ShowAlways = true;
            toolTipHelp.UseAnimation = true;
            toolTipHelp.UseFading = true;
            
            toolTipHelp.AutomaticDelay = 100;
            toolTipHelp.AutoPopDelay = 10000;
            toolTipHelp.InitialDelay = 100;
            toolTipHelp.ReshowDelay = 100;
            
            try
            {
                loadDescriptionDictionary();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadDescriptionDictionary()
        {            
            descriptionDictionary = InternalResourceReader.GetLocalResources();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Javascript files (*.js)|*.js|Coffee Script files (*.coffee)|*.coffee";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtInputFile.Text = openFileDialog1.FileName;
            }            
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "Javascript files (*.js)|*.js|Coffee Script files (*.coffee)|*.coffee";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtOutputFile.Text = saveFileDialog1.FileName;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            btnGenerate.Enabled = false;

            if (txtInputFile.Text.Length == 0)
            {
                MessageBox.Show("Please select an input file first");
                return;
            }
            if (txtOutputFile.Text.Length == 0)
            {
                MessageBox.Show("Please select an output file first");
                return;
            }

            // Parse file and generate controls
            try
            {
                Dictionary<string, TemplateFileItem> matchResultDictionary = TemplateFileParser.ParseFile(txtInputFile.Text);
                pnlReplacements.Controls.Clear();
                int padding = 5;
                int offsetY = 0;
                Regex keyRegex = new Regex(@"[^\w+]");

                //Geneate controls
                foreach (var mrKey in matchResultDictionary.Keys)
                {
                    string dictionaryKey = keyRegex.Replace(mrKey, "");
                    Panel itemRowPanel = new Panel();
                    itemRowPanel.Width = pnlReplacements.Width;
                    itemRowPanel.Height = 20;
                    itemRowPanel.Top = offsetY;
                    offsetY += itemRowPanel.Height;
                    int leftPos = padding;
                    
                    CheckBox chkBox = new CheckBox();
                    chkBox.Checked = true;
                    chkBox.Top = 0;
                    chkBox.Left = leftPos;
                    chkBox.Width = 20;
                    leftPos += chkBox.Width + padding;
                    itemRowPanel.Controls.Add(chkBox);

                    Label lblCounter = new Label();
                    lblCounter.Left = leftPos;
                    lblCounter.Top = padding;
                    lblCounter.Text = String.Format("{0} items", matchResultDictionary[mrKey].FilePositionList.Count().ToString());
                    lblCounter.Width = lblCounter.Width / 2;
                    leftPos += lblCounter.Width + padding;    
                    itemRowPanel.Controls.Add(lblCounter);                                    

                    TextBox txtBoxKey = new TextBox();
                    txtBoxKey.ReadOnly = true;
                    txtBoxKey.Text = dictionaryKey;
                    txtBoxKey.Top = 0;
                    txtBoxKey.Left = leftPos;
                    txtBoxKey.Width = 150;
                    leftPos += txtBoxKey.Width + padding;
                    itemRowPanel.Controls.Add(txtBoxKey);

                    TextBox txtReplacement = new TextBox();
                    txtReplacement.Top = 0;
                    txtReplacement.Left = leftPos;
                    txtReplacement.Width = 200;
                    txtReplacement.Tag = "ReplaceText";
                    itemRowPanel.Controls.Add(txtReplacement);
                                        
                    if (descriptionDictionary.ContainsKey(dictionaryKey))
                    {
                        toolTipHelp.SetToolTip(txtBoxKey, descriptionDictionary[dictionaryKey]);
                    }

                    itemRowPanel.Tag = matchResultDictionary[mrKey];
                    pnlReplacements.Controls.Add(itemRowPanel);
                }

                btnGenerate.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Dictionary<string, TemplateFileItem> replaceDictionary = new Dictionary<string, TemplateFileItem>();

            foreach (Control ctrl in pnlReplacements.Controls)
            {
                Panel itemPanel = ctrl as Panel;
                if (itemPanel != null)
                {
                    TemplateFileItem tfi = itemPanel.Tag as TemplateFileItem;
                    if (tfi != null)
                    {
                        foreach (Control innerControl in itemPanel.Controls)
                        {
                            if (innerControl is CheckBox)
                                tfi.Enabled = (innerControl as CheckBox).Checked;
                            else if (innerControl is TextBox && innerControl.Tag is string && (innerControl.Tag as string) == "ReplaceText")                            
                                tfi.Replacement = (innerControl as TextBox).Text;
                        }
                        replaceDictionary.Add(tfi.Variable, tfi);
                    }
                }
            }
            try
            {
                TemplateFileParser.GenerateOutput(txtInputFile.Text, txtOutputFile.Text, replaceDictionary);
                MessageBox.Show("File successfully generated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}