using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DatabaseImport.Models;

namespace DatabaseImport
{
    public class SQLHelper
    {
        public static string GetConnectionString(string server, string database, string username, string password, bool useWindowsAuth)
        {
            String connectionStr;
            if (useWindowsAuth)
                connectionStr = "Data Source=" + server + ";Integrated Security=SSPI;Initial Catalog=" + database + ";";
            else
                connectionStr = "Data Source=" + server + ";User ID=" + username + ";Password=" + password + ";Initial Catalog=" + database + ";";

            return connectionStr;
        }

        public static string GetConnectionString(string server, string username, string password, bool useWindowsAuth)
        {
            String connectionStr;
            if (useWindowsAuth)
                connectionStr = "Data Source=" + server + ";Integrated Security=SSPI;";
            else
                connectionStr = "Data Source=" + server + ";User ID=" + username + ";Password=" + password + ";";

            return connectionStr;
        }

        public static List<string> ListDatabases(string connectionString)
        {
            List<string> databaseList = new List<string>();

            SqlConnection conn = new SqlConnection {ConnectionString = connectionString};
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT name FROM sys.databases ORDER BY name", conn);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            conn.Close();

            foreach (DataRow row in dt.Rows)
                databaseList.Add(row[0] as String);

            return databaseList;
        }

        public static List<string> ListDatabaseTables(string connectionString, string databaseName)
        {
            if (!connectionString.Contains("Initial Catalog="))
                connectionString += "Initial Catalog=" + databaseName;
            
            SqlConnection conn = new SqlConnection { ConnectionString = connectionString };
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME", conn);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            conn.Close();

            return (from DataRow row in dt.Rows select row["TABLE_NAME"] as String).ToList();
        }

        public static List<SQLColumnStructure> ListTableColumns(string connectionString, string databaseName, string tableName)
        {
            if (!connectionString.Contains("Initial Catalog="))
                connectionString += "Initial Catalog=" + databaseName;

            SqlConnection conn = new SqlConnection { ConnectionString = connectionString };
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT object_id FROM sys.tables WHERE name='" + tableName + "'", conn);
            int tableObjectId = (int)cmd.ExecuteScalar();

            cmd = new SqlCommand("SELECT column_id, is_identity FROM sys.columns WHERE object_id=" + tableObjectId, conn);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dtIdentity = new DataTable();
            sqlDataAdapter.Fill(dtIdentity);

            Dictionary<int, bool> isIdentityDictionary = dtIdentity.Rows.Cast<DataRow>().ToDictionary(row => (int) row["column_id"], row => (bool) row["is_identity"]);

            cmd = new SqlCommand("SELECT COLUMN_NAME, ORDINAL_POSITION, COLUMN_DEFAULT, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME='" + tableName + "' ORDER BY ORDINAL_POSITION", conn);
            sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            conn.Close();

            return (from DataRow row in dt.Rows
                select new SQLColumnStructure
                {
                    CharacterMaximumLength = GetNullableDBValue<int?>(row[SQLColumnStructure.ColumnProperties.CHARACTER_MAXIMUM_LENGTH.ToString()]),
                    ColumnDefault = GetNullableDBValue<string>(row[SQLColumnStructure.ColumnProperties.COLUMN_DEFAULT.ToString()]),
                    ColumnName = (string) row[SQLColumnStructure.ColumnProperties.COLUMN_NAME.ToString()],
                    DataType = (string) row[SQLColumnStructure.ColumnProperties.DATA_TYPE.ToString()],
                    IsNullable = ((string) row[SQLColumnStructure.ColumnProperties.IS_NULLABLE.ToString()]) == "YES",
                    OrdinalPosition = (int) row[SQLColumnStructure.ColumnProperties.ORDINAL_POSITION.ToString()],
                    ReadOnly = isIdentityDictionary[(int)row[SQLColumnStructure.ColumnProperties.ORDINAL_POSITION.ToString()]]
                }).ToList();
        }

        private static T GetNullableDBValue<T>(object columnData)
        {
            if (columnData is DBNull || columnData == null)
                return default(T);

            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertFrom(typeof(string)))
                return (T)converter.ConvertFromString(columnData as string);
            
            if (converter.CanConvertFrom(typeof(Int32)))
                return (T) converter.ConvertFrom(columnData);
            
            return default(T);
        }
    }
}
