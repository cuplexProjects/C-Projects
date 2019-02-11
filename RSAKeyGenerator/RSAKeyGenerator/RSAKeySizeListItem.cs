namespace RSAKeyGenerator
{
    public class RSAKeySizeListItem
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public RSAKeySizeListItem()
        {
            
        }

        public RSAKeySizeListItem(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
