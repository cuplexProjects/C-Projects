using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbLib.linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Web;

namespace FileGetDbLib.Linq
{
    public class FgContext : FgDataClassesDataContext, IDisposable
    {
        public FgContext(System.Data.IDbConnection connection) : base(connection) 
        {
            _outerContext = _context_static;
            _context_static = this;
            this.CommandTimeout = 600;
        }

        private FgContext() : this(new System.Data.SqlClient.SqlConnection(DBHelper.GetConnectionString())) { }

        [Function(IsComposable = true)]
        [return: Parameter(DbType = "DateTime")]
        public System.DateTime GETDATE()
        {
            return (System.DateTime)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod()))).ReturnValue);
        }

        [ThreadStatic]
		private static FgContext _context_static = null;
		private FgContext _outerContext = null;
		private int _useCount = 0;

        public static FgContext Create()
        {
            return new FgContext();
        }

        static FgContext()
        {
            ignoreloglist.Add(typeof(EventLog),null);
        }

		public static FgContext CreateReuse()
		{
			if (_context_static == null)
				return FgContext.Create();

			_context_static._useCount++;
			return _context_static;
		}


        private static Dictionary<Type,object> ignoreloglist = new Dictionary<Type,object>();
        

        private static Dictionary<Type, System.Reflection.PropertyInfo[]> _typeToPrimaryKeys = new Dictionary<Type, System.Reflection.PropertyInfo[]>();

        private System.Reflection.PropertyInfo[] GetPrimaryKeys(Type type)
        {
            if(!_typeToPrimaryKeys.ContainsKey(type))
                _typeToPrimaryKeys[type] = type.GetProperties().Where(pi => pi.GetCustomAttributes(typeof(System.Data.Linq.Mapping.ColumnAttribute), false).Select(p => ((System.Data.Linq.Mapping.ColumnAttribute)p).IsPrimaryKey).FirstOrDefault()).ToArray();
            return _typeToPrimaryKeys[type];
        }

        private string GetAppname()
        {
            string appname = "Application Name=";
            int index = this.Connection.ConnectionString.IndexOf(appname, StringComparison.InvariantCultureIgnoreCase);
            if (index == -1)
            {
                appname = "App=";
                index = this.Connection.ConnectionString.IndexOf(appname, StringComparison.InvariantCultureIgnoreCase);
            }
            if (index >= 0)
            {
                index += appname.Length;
                int end = this.Connection.ConnectionString.IndexOf(';', index);
                if (end == -1)
                    end = this.Connection.ConnectionString.Length;
                return "(" + this.Connection.ConnectionString.Substring(index, end - index) + ") ";
            }
            return "";
        }

        private void AppendValue(StringBuilder sb, object value)
        {
            if (value == null)
            {
                sb.Append("null");
            }
            else if (value is string)
            {
                string stringValue = (string)value;
                sb.Append("'");
                if (stringValue.Length > 25)
                {
                    sb.Append(stringValue, 0, 25);
                    sb.Append("...");
                }
                else
                {
                    sb.Append((string)value);
                    sb.Append("'");
                }
            }
            else if (value is DateTime)
            {
                DateTime dateValue = (DateTime)value;
                if(dateValue.Date == dateValue)
                    sb.Append(((DateTime)value).ToString("yyyy-MM-dd"));
                else
                    sb.Append(((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss"));
            }
            else
            {
                sb.Append(value);
            }
        }

        protected static string _logfile = null;
        protected static string _currentLogfileName = "";
        protected static StreamWriter _currentLogStream = null;
        protected static System.Threading.Timer _flushTimer = null;

        protected bool IsLogEnabled
        {
            get
            {
                if (_logfile == null)
                {
                    _logfile = "";
                    if (HttpContext.Current != null)
                    {
                        _logfile = System.Configuration.ConfigurationManager.AppSettings["WebDatabaseLog"] ?? "";
                    }
                }
                return !string.IsNullOrEmpty(_logfile);
            }
        }      

        public System.Data.SqlClient.SqlCommand CreateSqlCommand()
        {
            var cmd = (System.Data.SqlClient.SqlCommand)this.Connection.CreateCommand();
            if (cmd.Connection.State == System.Data.ConnectionState.Closed)
            {
                cmd.Connection.Open();
                cmd.Disposed += delegate(object sender, EventArgs e)
                {
                    cmd.Connection.Close();
                };
            }
            return cmd;
        }

		new public void Dispose()
		{
			if(this._useCount > 0)
			{
				this._useCount--;
			}
			else
			{
				_context_static = _outerContext;
				base.Dispose();
			}
		}
    }
}
