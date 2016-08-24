using System;
using System.Collections.Generic;
using System.Linq;

namespace CuplexApiCommon.BulkCopyModels
{
    public class BulkCopyCollection : IBulkCopyCollection
    {
        public string TableName { get; protected set; }
        public int CommitSize { get; protected set; }
        private BulkCopyColumnDefinition[] bulkCopyColumnDefinitions;

        private BulkCopyCollection(string tableName, int commitSize, List<ColumnDefinition> columnDefinitions)
        {
            BulkCopyItems= new List<IBulkCopyItem>();
            TableName = tableName;
            CommitSize = commitSize;
            try
            {
                InitializeColumnDefinitions(columnDefinitions);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            
        }

        private void InitializeColumnDefinitions(List<ColumnDefinition> columnDefinitions)
        {
            bulkCopyColumnDefinitions=new BulkCopyColumnDefinition[columnDefinitions.Count];
            int index = 0;
            foreach (ColumnDefinition columnDefinition in columnDefinitions.OrderBy(cd=>cd.Index))
            {
                bulkCopyColumnDefinitions[index++] = new BulkCopyColumnDefinition(columnDefinition.ColumnName, columnDefinition.ColumnDataType);
            }
        }

        public BulkCopyColumnDefinition[] GetColumnDefinitions()
        {
            return bulkCopyColumnDefinitions;
        }

        public List<IBulkCopyItem> BulkCopyItems { get; private set; }

        public static BulkCopyCollection Create(string tableName, int commitSize, List<ColumnDefinition> columnDefinitions)
        {
            var bulkCopyCollection = new BulkCopyCollection(tableName, commitSize, columnDefinitions);
            return bulkCopyCollection;
        }
    }
}