namespace GeneralToolkitLib.Storage.Registry
{
    public interface IRegistryAccess
    {
        bool ShowError { get; set; }

        /// <summary>
        ///     A property to set the SubKey value
        ///     (default = "SOFTWARE\\" + Application.ProductName
        /// </summary>
        string SubKey { get; }

        void SetupSubKeyPathAndAccessRights();
        T ReadObjectFromRegistry<T>() where T : new();
        void SaveObjectToRegistry(object objToSave);
        bool DeleteKey(string keyName);

        /// <summary>
        ///     To delete a sub key and any child.
        ///     input: void
        ///     output: true or false
        /// </summary>
        bool DeleteSubKeyTree();

        /// <summary>
        ///     Retrive the count of subkeys at the current key.
        ///     input: void
        ///     output: number of subkeys
        /// </summary>
        int SubKeyCount();

        /// <summary>
        ///     Retrive the count of values in the key.
        ///     input: void
        ///     output: number of keys
        /// </summary>
        int ValueCount();
    }
}