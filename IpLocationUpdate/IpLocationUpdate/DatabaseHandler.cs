using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace IpLocationUpdate
{
    public class DatabaseHandler
    {
        public delegate void ProgressCallback(double percentDone, int itemsInserted);
        private readonly SqlConnection conn;
        private AuthtenticationType auth = AuthtenticationType.WindowsAuthentication;
        private const string INVALID_TABLE_STRUCTURE_ERROR = "Invalid table structure";
        private bool run_db_inport;

        public DatabaseHandler()
        {
            conn = new SqlConnection();
        }
        ~DatabaseHandler()
        {
            CloseConnection();
        }        
        public enum AuthtenticationType
        {
            WindowsAuthentication,
            SQLServerAuthentication
        }

        public AuthtenticationType Auth
        {
            get { return auth; }
            set { auth = value; }
        }

        public string OpenConnection(string serverName)
        {
            this.Auth = AuthtenticationType.WindowsAuthentication;
            try
            {
                conn.ConnectionString = "Data Source=" + serverName + ";Integrated Security=true;";
                conn.Open();
            }
            catch (Exception ex) { return ex.Message; }
            return "Connection open";
        }
        public string OpenConnection(string serverName, string userName, string password)
        {
            this.Auth = AuthtenticationType.SQLServerAuthentication;
            try
            {
                conn.ConnectionString = "Data Source=" + serverName + ";Integrated Security=false;User ID=" +
                    userName + ";Password=" + password + ";";
                conn.Open();
            }
            catch (Exception ex) { return ex.Message; }
            return "Connection open";
        }
        public void CloseConnection()
        {
            if (this.conn.State == ConnectionState.Open)
            {
                try { this.conn.Close(); }
                catch { }
            }
        }
        public List<string> GetDatabaseList()
        {
            List<string> dbList = new List<string>();
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("SELECT name FROM sys.databases ORDER BY name", this.conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        dbList.Add(row[0].ToString());
                    }
                }
            }
            catch { }
            return dbList;
        }
        public List<string> GetDatabaseTables(string database)
        {
            List<string> tblList = new List<string>();
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    this.conn.ChangeDatabase(database);
                    SqlCommand command = new SqlCommand("SELECT name FROM sys.tables ORDER BY name", this.conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    tblList.AddRange(from DataRow row in dt.Rows select row[0].ToString());
                }
            }
            catch { }
            return tblList;
        }

        public async Task<string> InsertIpLocationListAsync(List<IpLocation> ipLocationList, string database, string table, ProgressCallback callback)
        {
            Task<string> getStringTask = InsertIpLocationListTask(ipLocationList, database, table, callback);

            string resultText = await getStringTask;
            return resultText;
        }

        private Task<string> InsertIpLocationListTask(List<IpLocation> ipLocationList, string database, string table, ProgressCallback callback)
        {
            return Task.Run(() => InsertIpLocationList(ipLocationList, database, table, callback));
        }

        public void AbortDbInsertOperation()
        {
            run_db_inport = false;
        }

        public string InsertIpLocationList(List<IpLocation> ipLocationList, string database, string table, ProgressCallback callback)
        {
            int rowsInserted = 0;
            run_db_inport = true;
            try
            {
                //Basic verification of table structure
                this.conn.ChangeDatabase(database);
                SqlCommand command = new SqlCommand(@"SELECT TABLE_CATALOG,TABLE_NAME, COLUMN_NAME, IS_NULLABLE, DATA_TYPE 
                    FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_CATALOG = '" + database + "' AND TABLE_NAME = '" + table + "' ", this.conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                List<DbColumnStructure> colStructureList = new List<DbColumnStructure>();
                foreach (DataRow row in dt.Rows)
                {
                    DbColumnStructure colStructure = new DbColumnStructure();
                    colStructure.ColumnName = (string)row["COLUMN_NAME"];
                    colStructure.DataType = (string)row["DATA_TYPE"];
                    colStructure.IsNullable = (string)row["IS_NULLABLE"];

                    colStructureList.Add(colStructure);
                }

                if (colStructureList.Count != 7)
                    return INVALID_TABLE_STRUCTURE_ERROR;
                                
                if (!colStructureList.Contains(new DbColumnStructure("IPFrom", "NO", "bigint")))
                    return INVALID_TABLE_STRUCTURE_ERROR;
                if (!colStructureList.Contains(new DbColumnStructure("IPTo", "NO", "bigint")))
                    return INVALID_TABLE_STRUCTURE_ERROR;
                if (!colStructureList.Contains(new DbColumnStructure("Registry", "NO", "varchar")))
                    return INVALID_TABLE_STRUCTURE_ERROR;
                if (!colStructureList.Contains(new DbColumnStructure("Assigned", "NO", "bigint")))
                    return INVALID_TABLE_STRUCTURE_ERROR;
                if (!colStructureList.Contains(new DbColumnStructure("Ctry", "NO", "varchar")))
                    return INVALID_TABLE_STRUCTURE_ERROR;
                if (!colStructureList.Contains(new DbColumnStructure("Cntry", "NO", "varchar")))
                    return INVALID_TABLE_STRUCTURE_ERROR;
                if (!colStructureList.Contains(new DbColumnStructure("Country", "NO", "varchar")))
                    return INVALID_TABLE_STRUCTURE_ERROR;

                command.CommandText = "TRUNCATE TABLE " + table;
                command.ExecuteNonQuery();

                int itemsInserted = 0;
                foreach (IpLocation ipLoc in ipLocationList)
                {
                    string insertStatement = "INSERT INTO " + table + "([IPFrom],[IPTo],[Registry],[Assigned],[Ctry],[Cntry],[Country]) VALUES(@v1,@v2,@v3,@v4,@v5,@v6,@v7)";
                    command.CommandText = insertStatement;
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@v1", ipLoc.IpFrom);
                    command.Parameters.AddWithValue("@v2", ipLoc.IpTo);
                    command.Parameters.AddWithValue("@v3", ipLoc.Registry);
                    command.Parameters.AddWithValue("@v4", ipLoc.Assigned);
                    command.Parameters.AddWithValue("@v5", ipLoc.Ctry);
                    command.Parameters.AddWithValue("@v6", ipLoc.Cntry);
                    command.Parameters.AddWithValue("@v7", ipLoc.Country);

                    rowsInserted += command.ExecuteNonQuery();
                    itemsInserted++;

                    if (callback != null && rowsInserted % 10 == 0)
                        callback.Invoke((rowsInserted / (double)ipLocationList.Count) * 100, itemsInserted);

                    if (!run_db_inport)
                        break;
                }
            }
            catch (Exception ex) { return ex.Message; }
            return "Inserted " + rowsInserted + " rows";
        }
        public bool ConnectionIsOpen()
        {
            return conn.State == ConnectionState.Open;
        }

        public static async Task<List<SQLServerInstance>> GetLocalSQLServerInstancesAsync()
        {
            List<SQLServerInstance> sqlServerList = await Task.Run(() => GetLocalSQLServerInstances());
            return sqlServerList;
        }

        public static List<SQLServerInstance> GetLocalSQLServerInstances()
        {
            //DataTable dt = SqlDataSourceEnumerator.Instance.GetDataSources();
            //foreach (DataRow row in dt.Rows)
            //{
            //    bool isClustred = ConvertToString(row[2]).ToLower() != "no";
            //    sqlServerList.Add(new SQLServerInstance(ConvertToString(row[0]), ConvertToString(row[1]), isClustred, ConvertToString(row[3])));
            //}

            string[] sqlServers = SQLServerEnumerationHandler.GetServers();
            return sqlServers.Select(sqlServer => new SQLServerInstance(sqlServer)).ToList();
        }
        private static string ConvertToString(object dbStr)
        {
            var s = dbStr as string;
            if (s != null)
                return s;
            return "";
        }
    }
    public class SQLServerInstance
    {
        public string ServerName { get; private set; }
        public string InstanceName { get; private set; }
        public bool IsClustered { get; private set; }
        public string Version { get; private set; }

        public SQLServerInstance(string serverName, string instanceName, bool isClustred, string version)
        {
            this.ServerName = serverName;
            this.InstanceName = instanceName;
            this.IsClustered = isClustred;
            this.Version = version;
        }

        public SQLServerInstance(string serverName)
        {
            this.ServerName = serverName;
            this.InstanceName = "";
            this.IsClustered = false;
            this.Version = "Unknown";
        }

        public override string ToString()
        {
            return this.ServerName + "\\" + this.InstanceName;
        }
    }

    public class DbColumnStructure : IEquatable<DbColumnStructure>
    {
        public string ColumnName { get; set; }
        public string IsNullable { get; set; }
        public string DataType { get; set; }

        public DbColumnStructure() { }
        public DbColumnStructure(string columnName, string isNullable, string dataType)
        {
            this.ColumnName = columnName;
            this.IsNullable = isNullable;
            this.DataType = dataType;
        }
        public bool Equals(DbColumnStructure other)
        {
            return this.ColumnName == other.ColumnName && this.DataType == other.DataType && this.IsNullable == other.IsNullable;
        }     
    }
}