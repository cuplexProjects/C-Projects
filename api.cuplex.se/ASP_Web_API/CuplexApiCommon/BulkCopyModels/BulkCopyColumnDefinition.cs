using System;

namespace CuplexApiCommon.BulkCopyModels
{
    public class BulkCopyColumnDefinition
    {
        public BulkCopyColumnDefinition(string columnName, Type dataType)
        {
            ColumnName = columnName;
            DataType = dataType;

            bool isNullable = DataType.IsGenericType && DataType.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (isNullable)
            {
                var underlyingType = Nullable.GetUnderlyingType(DataType);
                DataType = underlyingType ?? DataType;
            }
            else
                DataType = DataType.UnderlyingSystemType;

        }

        public string ColumnName { get; private set; }
        public Type DataType { get; private set; }

    }
}