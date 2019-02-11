namespace DatabaseImport.Models
{
    public class DatabaseImportSettings
    {
        public string TableName { get; set; }
        public string ConnectionString { get; set; }
        public DatabaseColumnCoupling ColumnValues { get; set; }
    }
}
