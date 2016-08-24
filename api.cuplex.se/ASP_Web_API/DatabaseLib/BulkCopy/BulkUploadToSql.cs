using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using CuplexApiCommon.BulkCopyModels;

namespace DatabaseLib.BulkCopy
{
    public class BulkUploadToSql
    {
        protected readonly IEnumerable<BulkCopyColumnDefinition> bulkCopyColumnDefinitions;
        protected readonly int commitBatchSize;
        protected readonly DataTable dataTable;
        private readonly List<IBulkCopyItem> internalStore;
        protected readonly string tableName;
        protected int RecordCount;

        private BulkUploadToSql(string tableName, int commitBatchSize, IEnumerable<BulkCopyColumnDefinition> bulkCopyColumnDefinitions)
        {
            internalStore = new List<IBulkCopyItem>();

            this.tableName = tableName;
            dataTable = new DataTable(tableName);
            RecordCount = 0;
            this.commitBatchSize = commitBatchSize;
            this.bulkCopyColumnDefinitions = bulkCopyColumnDefinitions;

            // add columns to this data table
            InitializeStructures();
        }

        private void InitializeStructures()
        {
            foreach (BulkCopyColumnDefinition columnDefinition in bulkCopyColumnDefinitions)
            {
                dataTable.Columns.Add(columnDefinition.ColumnName, columnDefinition.DataType);
            }
        }

        public static BulkUploadToSql Load(IBulkCopyCollection bulkCopyCollection, Action<double> setProgress)
        {
            // create a new object to return
            var o = new BulkUploadToSql(bulkCopyCollection.TableName, bulkCopyCollection.CommitSize, bulkCopyCollection.GetColumnDefinitions());

            // replace the code below
            // with your custom logic 
            Stopwatch stopwatch= new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < bulkCopyCollection.BulkCopyItems.Count; i++)
            {
                if (stopwatch.ElapsedMilliseconds >= 100)
                {
                    setProgress((double)i/bulkCopyCollection.BulkCopyItems.Count);
                    stopwatch.Restart();
                }
                IBulkCopyItem rec = bulkCopyCollection.BulkCopyItems[i];
                o.internalStore.Add(rec);
            }
            stopwatch.Stop();
            return o;
        }

        public void Flush()
        {
            // transfer data to the datatable
            foreach (IBulkCopyItem rec in internalStore)
            {
                PopulateDataTable(rec);
                if (RecordCount >= commitBatchSize)
                    WriteToDatabase();
            }
            // write remaining records to the DB
            if (RecordCount > 0)
                WriteToDatabase();
        }

        private void WriteToDatabase()
        {
            // get your connection string
            string connString = DBHelper.GetConnectionString();
            // connect to SQL
            using (var connection =
                new SqlConnection(connString))
            {
                // make sure to enable triggers
                // more on triggers in next post
                var bulkCopy =
                    new SqlBulkCopy
                        (
                        connection,
                        SqlBulkCopyOptions.TableLock |
                        SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction,
                        null
                        );

                // set the destination table name
                bulkCopy.DestinationTableName = tableName;
                connection.Open();

                // write the data in the "dataTable"
                bulkCopy.WriteToServer(dataTable);
                connection.Close();
            }
            // reset
            dataTable.Clear();
            //this.recordCount = 0;
        }

        private void PopulateDataTable(IBulkCopyItem record)
        {
            // populate the values
            // using your custom logic
            DataRow row = dataTable.NewRow();

            for (int i = 0; i < record.RowObjects.Count(); i++)
            {
                var rec= record.RowObjects.ElementAt(i) ?? DBNull.Value;

                row[i] = rec;
            }

            // add it to the base for final addition to the DB
            dataTable.Rows.Add(row);
            RecordCount++;
        }
    }
}