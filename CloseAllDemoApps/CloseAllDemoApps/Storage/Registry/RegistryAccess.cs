using System;
using System.Reflection;
using Microsoft.Win32;

namespace CloseAllDemoApps.Storage.Registry
{
    public class RegistryAccess
    {
        private RegistryKey _baseRegistryKey = Microsoft.Win32.Registry.LocalMachine;
        private bool _showError;
        private readonly string _subKey;


        public RegistryAccess(string productName)
        {
            ProductName = productName;
            _subKey = "SOFTWARE\\" + ProductName;
        }

        public string ProductName { get; private set; }

        public bool ShowError
        {
            get { return this._showError; }
            set { this._showError = value; }
        }

        /// <summary>
        /// A property to set the SubKey value
        /// (default = "SOFTWARE\\" + Application.ProductName
        /// </summary>
        public string SubKey
        {
            get { return this._subKey; }
        }

        /// <summary>
        /// A property to set the BaseRegistryKey value.
        /// (default = Registry.LocalMachine)
        /// </summary>
        public RegistryKey BaseRegistryKey
        {
            get { return this._baseRegistryKey; }
            set { this._baseRegistryKey = value; }
        }

        public T ReadObjectFromRegistry<T>() where T : new()
        {
            try
            {
                T retVal = new T();
                Type objType = typeof(T);
                PropertyInfo[] properties = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo propertyInfo in properties)
                {
                    string propertyName = propertyInfo.Name;
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        propertyInfo.SetValue(retVal, this.Read(propertyName) as string);
                    }
                    else if (propertyInfo.PropertyType == typeof(bool))
                    {
                        propertyInfo.SetValue(retVal, (this.Read(propertyName) as string) == "true");
                    }
                    else if (propertyInfo.PropertyType == typeof(int))
                    {
                        object tmp = this.Read(propertyName);
                        if(tmp != null)
                            propertyInfo.SetValue(retVal, (int)tmp);
                    }
                    else if (propertyInfo.PropertyType.BaseType == typeof(Enum))
                    {
                        object tmp = this.Read(propertyName);
                        if(tmp != null)
                        {
                            int enValue = (int)tmp;
                            propertyInfo.SetValue(retVal, enValue);
                        }
                    }
                }

                return retVal;
            }
            catch(Exception e)
            {
                this.ShowErrorMessage(e, "SaveObjectToRegistry");
                return default(T);
            }
        }

        public void SaveObjectToRegistry(object objToSave)
        {
            try
            {
                Type objType = objToSave.GetType();
                PropertyInfo[] properties = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo propertyInfo in properties)
                {
                    string propertyName = propertyInfo.Name;
                    RegistryDataTypes registryData = null;
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        registryData = new RegistryDataTypeString {KeyName = propertyName, Data = propertyInfo.GetValue(objToSave)};
                    }
                    else if (propertyInfo.PropertyType == typeof(int))
                    {
                        registryData = new RegistryDataTypeDWORD {KeyName = propertyName, Data = propertyInfo.GetValue(objToSave)};
                    }
                    else if (propertyInfo.PropertyType == typeof(bool))
                    {
                        registryData = new RegistryDataTypeString {KeyName = propertyName, Data = ((bool)propertyInfo.GetValue(objToSave)) ? "true" : "false"};
                    }
                    else if(propertyInfo.PropertyType.BaseType == typeof(Enum))
                    {
                        registryData = new RegistryDataTypeDWORD {KeyName = propertyName, Data = (int)propertyInfo.GetValue(objToSave)};
                    }
                    if(registryData != null)
                        Write(registryData);
                }

            }
            catch(Exception e)
            {
                this.ShowErrorMessage(e, "SaveObjectToRegistry");
            }
        }

        public object Read(string keyName)
        {
            // Opening the registry key
            RegistryKey rk = this._baseRegistryKey;
            // Open a subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey(this._subKey);
            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                return null;
            }
            try
            {
                return sk1.GetValue(keyName);
            }
            catch (Exception e)
            {
                this.ShowErrorMessage(e, "Reading registry " + keyName);
                return null;
            }
        }


        public bool Write(RegistryDataTypes registryDataEntry)
        {
            if (registryDataEntry.Data == null) return false;

            try
            {
                RegistryKey rk = this._baseRegistryKey;
                RegistryKey sk1 = rk.CreateSubKey(this._subKey);

                if (sk1 != null) sk1.SetValue(registryDataEntry.KeyName, registryDataEntry.Data, registryDataEntry.DataType);

                return true;
            }
            catch (Exception e)
            {
                this.ShowErrorMessage(e, "Writing registry " + registryDataEntry.KeyName);
                return false;
            }
        }

        public bool DeleteKey(string keyName)
        {
            try
            {
                // Setting
                RegistryKey rk = this._baseRegistryKey;
                RegistryKey sk1 = rk.CreateSubKey(this._subKey);
                // If the RegistrySubKey doesn't exists -> (true)
                if (sk1 == null)
                    return true;
                sk1.DeleteValue(keyName);

                return true;
            }
            catch (Exception e)
            {
                this.ShowErrorMessage(e, "Deleting SubKey " + this._subKey);
                return false;
            }
        }


        /// <summary>
        /// To delete a sub key and any child.
        /// input: void
        /// output: true or false 
        /// </summary>
        public bool DeleteSubKeyTree()
        {
            try
            {
                // Setting
                RegistryKey rk = this._baseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(this._subKey);
                // If the RegistryKey exists, I delete it
                if (sk1 != null)
                    rk.DeleteSubKeyTree(this._subKey);

                return true;
            }
            catch (Exception e)
            {
                this.ShowErrorMessage(e, "Deleting SubKey " + this._subKey);
                return false;
            }
        }


        /// <summary>
        /// Retrive the count of subkeys at the current key.
        /// input: void
        /// output: number of subkeys
        /// </summary>
        public int SubKeyCount()
        {
            try
            {
                // Setting
                RegistryKey rk = this._baseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(this._subKey);
                // If the RegistryKey exists...
                if (sk1 != null)
                    return sk1.SubKeyCount;
                return 0;
            }
            catch (Exception e)
            {
                this.ShowErrorMessage(e, "Retriving subkeys of " + this._subKey);
                return 0;
            }
        }


        /// <summary>
        /// Retrive the count of values in the key.
        /// input: void
        /// output: number of keys
        /// </summary>
        public int ValueCount()
        {
            try
            {
                // Setting
                RegistryKey rk = this._baseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(this._subKey);
                // If the RegistryKey exists...
                if (sk1 != null)
                    return sk1.ValueCount;
                return 0;
            }
            catch (Exception e)
            {
                this.ShowErrorMessage(e, "Retriving keys of " + this._subKey);
                return 0;
            }
        }

        private void ShowErrorMessage(Exception e, string title)
        {
            //if (this._showError)
            //    LogWriter.WriteLog("ModifyRegistry - Error: " + title + "\n" + e.Message);
        }
    }
}