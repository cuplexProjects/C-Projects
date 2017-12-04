using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Serilog;
using Serilog.Enrichers;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace Apps.cuplex.se
{
    public static class SerilogConfig
    {
        public static void ConfigureSerilog()
        {
            var columnOptions = new ColumnOptions
            {
                Properties =
                {
                    ExcludeAdditionalProperties = false,
                    OmitElementIfEmpty = false
                },
                AdditionalDataColumns = new Collection<DataColumn>
                {
                    new DataColumn {DataType = typeof(string), ColumnName = MachineNameEnricher.MachineNamePropertyName},
                    new DataColumn {DataType = typeof(string), ColumnName = "MethodName"},
                    new DataColumn {DataType = typeof(string), ColumnName = "SourceContext"}
                }
            };
            var logEventLevel = LogEventLevel.Information;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(HttpContext.Current.Server.MapPath("/")+ "Logfiles/apps.cuplex_Log.txt", rollingInterval: RollingInterval.Day)
                //.WriteTo.MSSqlServer("ErrorLog", "WebsiteLogs", LogEventLevel.Information)
                .WriteTo.MSSqlServer("ErrorLog", "WebsiteLogs", logEventLevel, period: TimeSpan.FromSeconds(5), batchPostingLimit: 5, columnOptions: columnOptions)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Destructure.ByTransforming<SqlCommand>(r => new { r.CommandText, r.Parameters })
                    .CreateLogger();
       
        }
    }
}