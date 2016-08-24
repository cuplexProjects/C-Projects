using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GeneralToolkitLib.Storage.Filesystem;
using SQLite;

namespace XmlToSqliteImport
{
    public partial class FormMain : Form
    {
        private XMLImporter xmlImporter;
        public FormMain()
        {
            this.InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if(this.xmlImporter == null || this.xmlImporter.XMLDocumentNodes == null)
            {
                MessageBox.Show("Please load xml first");
                return;
            }

            this.openFileDialog1.Filter = "Sqlite db files(*.db)|*.db";
            this.openFileDialog1.FileName = "database.db";

            if(this.openFileDialog1.ShowDialog(this) != DialogResult.OK)
                return;

            string path = this.openFileDialog1.FileName;

            try
            {
                using (var conn = new SQLite.SQLiteConnection(path, SQLiteOpenFlags.ReadWrite, true))
                {
                    List<SQLiteConnection.ColumnInfo> tableInfo = conn.GetTableInfo("sms");

                    StringBuilder sbColumnNames = new StringBuilder();
                    StringBuilder sbColumnValues = new StringBuilder();

                    int i = 1;
                    foreach (SQLiteConnection.ColumnInfo sqliteColumn in tableInfo)
                    {
                        sbColumnNames.Append(sqliteColumn.Name + ",");
                        sbColumnValues.Append("@a"+ i +",");
                        i++;
                    }

                    string query = "INSERT INTO sms (" + sbColumnNames.ToString().Trim(',') + ") VALUES (" + sbColumnValues.ToString().Trim(',') + ")";

                    foreach (XMLDataElement xmlDocumentNode in this.xmlImporter.XMLDocumentNodes)
                    {
                        SQLiteCommand insertSQL = new SQLiteCommand(conn) {CommandText = query};

                        i = 1;
                        foreach (SQLiteConnection.ColumnInfo sqliteColumn in tableInfo)
                        {
                            if(sqliteColumn.Name == "body")
                                insertSQL.Bind("@a" + i, xmlDocumentNode.ElementValue);
                            else
                                insertSQL.Bind("@a" + i, xmlDocumentNode.ElementProperties[sqliteColumn.Name]);
                            i++;
                        }

                        insertSQL.ExecuteNonQuery();
                        
                    }

                    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnImportXML_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "XML file(*.xml)|*.xml";
            this.openFileDialog1.FileName = "xmlfile.xml";

            if (this.openFileDialog1.ShowDialog(this) != DialogResult.OK)
                return;

            string path = this.openFileDialog1.FileName;

            try
            {
                this.xmlImporter = new XMLImporter();
                this.xmlImporter.LoadXMLFile(path);

                this.dataGridViewXML.ColumnCount = this.xmlImporter.XMLDocumentNodes[0].ElementProperties.Count + 2;
                this.dataGridViewXML.Columns[0].Name = "Element type";
                this.dataGridViewXML.Columns[1].Name = "Element InnerText";
                for (int i = 0; i < this.xmlImporter.XMLDocumentNodes[0].ElementProperties.Keys.Count; i++)
                {
                    this.dataGridViewXML.Columns[i+2].Name = this.xmlImporter.XMLDocumentNodes[0].ElementProperties.Keys[i];
                }

                foreach (var xmlDocumentNode in this.xmlImporter.XMLDocumentNodes)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = xmlDocumentNode.ElementType });
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = xmlDocumentNode.ElementValue });
                    foreach (string attribute in xmlDocumentNode.ElementProperties.AllKeys)
                    {
                        DataGridViewCell cell = new DataGridViewTextBoxCell();
                        cell.Value = xmlDocumentNode.ElementProperties[attribute];
                        row.Cells.Add(cell);
                    }
                    
                    
                    this.dataGridViewXML.Rows.Add(row);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
