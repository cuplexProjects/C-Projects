using System;

namespace CuplexApiCommon.BulkCopyModels
{
    public class ColumnDefinition
    {
        public int Index { get; set; }
        public string ColumnName { get; set; }
        public Type ColumnDataType{ get; set; }
    }
}
