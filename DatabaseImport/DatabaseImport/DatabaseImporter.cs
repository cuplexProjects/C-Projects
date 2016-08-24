using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseImport.Delegates;
using DatabaseImport.Models;

namespace DatabaseImport
{
    public class DatabaseImporter
    {
        private readonly DatabaseImportSettings importSettings;
        private readonly ProgressCallback progressCallback;

        public DatabaseImporter(DatabaseImportSettings databaseImportSettings, ProgressCallback progressCallback)
        {
            this.importSettings = databaseImportSettings;
            this.progressCallback = progressCallback;
        }

        public async Task RunDatabaseImportAsync(string importFile, string delimiter)
        {
            List<string> importItemList = await ParseImportFileAsync(importFile, delimiter);
            SqlConnection conn = new SqlConnection { ConnectionString = importSettings.ConnectionString };
            conn.Open();

            //Build sql statemant
            string sqlCmd = "INSERT INTO" + importSettings.TableName + " VALUES()";

            foreach (string importItem in importItemList)
            {
                SqlCommand cmd = new SqlCommand("", conn);
            }
            conn.Close();
        }

        private async Task<List<string>> ParseImportFileAsync(string importFile, string delimiter)
        {
            return await Task.Run(() => ParseImportFile(importFile, delimiter));
        }

        private List<string> ParseImportFile(string importFile, string delimiter)
        {
            List<string> importItemList = new List<string>();

            FileStream fs = File.OpenRead(importFile);
            StreamReader sr = new StreamReader(fs);

            while (fs.Position<fs.Length)
            {
                string importRow = sr.ReadLine();
                importItemList.Add(importRow);
            }

            return importItemList;
        }
    }
}