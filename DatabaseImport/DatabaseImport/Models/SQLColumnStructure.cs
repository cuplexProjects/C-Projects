using System.ComponentModel;

namespace DatabaseImport.Models
{
    public class SQLColumnStructure
    {
        [BrowsableAttribute(false)]
        public enum ColumnProperties
        {
            COLUMN_NAME,
            ORDINAL_POSITION,
            COLUMN_DEFAULT,
            IS_NULLABLE,
            DATA_TYPE,
            CHARACTER_MAXIMUM_LENGTH
        };

        [BrowsableAttribute(false)]
        public bool ReadOnly { get; set; }
        public string ColumnName { get; set; }
        public int OrdinalPosition { get; set; }
        public string ColumnDefault { get; set; }
        public bool IsNullable { get; set; }
        public string DataType { get; set; }
        public int? CharacterMaximumLength { get; set; }
    }
}