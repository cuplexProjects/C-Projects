using System.Collections.Generic;

namespace CuplexApiCommon.BulkCopyModels
{
    public interface IBulkCopyCollection
    {
        string TableName { get; }
        int CommitSize { get; }
        BulkCopyColumnDefinition[] GetColumnDefinitions();

        List<IBulkCopyItem> BulkCopyItems { get; }
    }

    public interface IBulkCopyItem
    {
        IEnumerable<object> RowObjects { get; set; }
    }
}
