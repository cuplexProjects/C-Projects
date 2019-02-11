using Microsoft.Win32;

namespace CloseAllDemoApps.Storage.Registry
{
    public abstract class RegistryDataTypes
    {
        public abstract string KeyName { get; set; }
        public abstract object Data { get; set; }
        public abstract RegistryValueKind DataType { get; }
    }
}