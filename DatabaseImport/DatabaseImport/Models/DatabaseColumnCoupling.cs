using System.ComponentModel;

namespace DatabaseImport.Models
{
    public class DatabaseColumnCoupling
    {
        [Browsable(false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
