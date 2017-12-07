using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using GeneralToolkitLib.Log;
using GeneralToolkitLib.WindowsApi;
using Microsoft.Win32;

namespace GeneralToolkitLib.Storage.Registry
{
    public class RegistryAccess
    {
        public RegistryAccess(string productName)
        {
            ProductName = productName;
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Invalid argument. productName cant be null or whitespace.", nameof(productName));

            SubKey = "SOFTWARE\\" + ProductName.Trim();
        }

        public RegistryAccess(string companyName, string productName)
        {
            CompanyName = companyName;
            ProductName = productName;

            if (string.IsNullOrWhiteSpace(companyName))
                throw new ArgumentException("Invalid argument. CompanyName cant be null or whitespace.", nameof(companyName));
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Invalid argument. productName cant be null or whitespace.", nameof(productName));

            SubKey = "SOFTWARE\\" + CompanyName.Trim() + "\\" + ProductName.Trim();
        }

        //private RegistryKey _baseRegKeyLocalMachine = Microsoft.Win32.Registry.LocalMachine;

        private string ProductName { get; }
        private string CompanyName { get; }
        public bool ShowError { get; set; }

        /// <summary>
        ///     A property to set the SubKey value
        ///     (default = "SOFTWARE\\" + Application.ProductName
        /// </summary>
        public string SubKey { get; }

        /// <summary>
        ///     A property to set the BaseRegistryKey value.
        ///     (default = Registry.LocalMachine)
        /// </summary>
        private RegistryKey BaseRegKeyCurrentUser { get; set; } = Microsoft.Win32.Registry.CurrentUser;

        public void SetupSubKeyPathAndAccessRights()
        {
            var pathArray = SubKey.Split('\\');
            RegistryKey baseRegistryKey = BaseRegKeyCurrentUser;
            //string ssid = WindowsIdentityHelper.GetUniqueSecurityIdForCurrentUser();
            WindowsIdentity windowsIdentity = WindowsIdentityHelper.GetWindowsIdentityForCurrentUser();

            foreach (string path in pathArray)
            {
                RegistryKey sk1 = baseRegistryKey.OpenSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree);
                bool userAccessGranted = true;
                if (sk1 == null)
                    sk1 = baseRegistryKey.CreateSubKey(path);

                if (sk1 == null)
                    userAccessGranted = false;


                if (userAccessGranted) continue;

                var registrySecurity = new RegistrySecurity();
                IdentityReference identityReference = new NTAccount(windowsIdentity.Name);
                var rule = new RegistryAccessRule(identityReference, RegistryRights.CreateSubKey | RegistryRights.ReadKey | RegistryRights.WriteKey, AccessControlType.Allow);
                registrySecurity.AddAccessRule(rule);
                sk1 = baseRegistryKey.OpenSubKey(path, RegistryKeyPermissionCheck.ReadSubTree);
                sk1?.SetAccessControl(registrySecurity);
            }
        }

        public T ReadObjectFromRegistry<T>() where T : new()
        {
            try
            {
                var retVal = new T();
                Type objType = typeof (T);
                var properties = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo propertyInfo in properties)
                {
                    string propertyName = propertyInfo.Name;
                    if (propertyInfo.PropertyType == typeof (string))
                        propertyInfo.SetValue(retVal, Read(propertyName) as string);
                    else if (propertyInfo.PropertyType == typeof (bool))
                        propertyInfo.SetValue(retVal, Read(propertyName) as string == "true");
                    else if (propertyInfo.PropertyType == typeof (int))
                    {
                        object tmp = Read(propertyName);
                        if (tmp != null)
                            propertyInfo.SetValue(retVal, (int) tmp);
                    }
                    else if (propertyInfo.PropertyType == typeof(long))
                    {
                        object tmp = Read(propertyName);
                        if (tmp != null)
                            propertyInfo.SetValue(retVal, (long)tmp);
                    }
                    else if (propertyInfo.PropertyType.BaseType == typeof (Enum))
                    {
                        object tmp = Read(propertyName);
                        if (tmp != null)
                        {
                            int enValue = (int) tmp;
                            propertyInfo.SetValue(retVal, enValue);
                        }
                    }
                    else if (propertyInfo.PropertyType == typeof (Size) || propertyInfo.PropertyType == typeof (Point))
                    {
                        try
                        {
                            propertyInfo.SetValue(retVal, DeSerializeStructFromString(Read(propertyName) as string));
                        }
                        catch (Exception ex)
                        {
                            LogWriter.LogError("ReadObjectFromRegistry Exception", ex);
                        }
                    }
                    else if (propertyInfo.PropertyType.BaseType == typeof (object))
                        if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.IsClass)
                        {
                            if (!typeof (IEnumerable).IsAssignableFrom(propertyInfo.PropertyType)) continue;
                            object regValObj = Read(propertyName);

                            if (!(regValObj is string)) continue;
                            var strArr = (regValObj as string).Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                            var regDataVal = strArr.ToList();
                            propertyInfo.SetValue(retVal, regDataVal);
                        }
                }

                return retVal;
            }
            catch (Exception e)
            {
                LogWriter.LogError("SaveObjectToRegistry", e);
                return default(T);
            }
        }

        public void SaveObjectToRegistry(object objToSave)
        {
            try
            {
                Type objType = objToSave.GetType();
                var properties = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo propertyInfo in properties)
                {
                    string propertyName = propertyInfo.Name;
                    RegistryDataTypes registryData = null;
                    if (propertyInfo.PropertyType == typeof (string))
                        registryData = new RegistryDataTypeString {KeyName = propertyName, Data = propertyInfo.GetValue(objToSave)};
                    else if (propertyInfo.PropertyType == typeof (int))
                        registryData = new RegistryDataTypeDWORD {KeyName = propertyName, Data = propertyInfo.GetValue(objToSave)};
                    else if (propertyInfo.PropertyType == typeof(long))
                        registryData = new RegistryDataTypeQWORD{ KeyName = propertyName, Data = propertyInfo.GetValue(objToSave) };
                    else if (propertyInfo.PropertyType == typeof (bool))
                        registryData = new RegistryDataTypeString {KeyName = propertyName, Data = (bool) propertyInfo.GetValue(objToSave) ? "true" : "false"};
                    else if (propertyInfo.PropertyType.BaseType == typeof (Enum))
                        registryData = new RegistryDataTypeDWORD {KeyName = propertyName, Data = (int) propertyInfo.GetValue(objToSave)};
                    else if (propertyInfo.PropertyType == typeof (Size) || propertyInfo.PropertyType == typeof (Point))
                    {
                        try
                        {
                            registryData = new RegistryDataTypeString {KeyName = propertyName, Data = SerializeStructToString(propertyInfo.GetValue(objToSave))};
                        }
                        catch (Exception ex)
                        {
                            LogWriter.LogError("SaveObjectToRegistry Exception", ex);
                        }
                    }
                    else if (propertyInfo.PropertyType.BaseType == typeof (object))
                        if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.IsClass)
                            if (typeof (IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                            {
                                var sb = new StringBuilder();
                                var collection = propertyInfo.GetValue(objToSave) as IEnumerable;
                                if (collection != null)
                                {
                                    foreach (object obj in collection)
                                    {
                                        sb.AppendLine(obj.ToString());
                                    }
                                    registryData = new RegistryDataTypeString {KeyName = propertyName, Data = sb.ToString()};
                                }
                            }
                    if (registryData != null)
                        Write(registryData);
                }
            }
            catch (Exception e)
            {
                LogWriter.LogError("SaveObjectToRegistry", e);
            }
        }

        private string SerializeStructToString(object obj)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField | BindingFlags.SetProperty).Where(p => p.CanRead && p.CanWrite).ToList();
            //string data = obj.GetType().FullName + "|";
            string data = GetCompleteTypeName(obj.GetType()) + "|";

            return properties.Aggregate(data, (current, propertyInfo) => current + (propertyInfo.Name + "=" + propertyInfo.GetValue(obj)) + ";");
        }

        private object DeSerializeStructFromString(string data)
        {
            int index = data.IndexOf("|", StringComparison.Ordinal);
            string objType = data.Substring(0, index);

            Type type = Type.GetType(objType);
            if (type != null)
            {
                object value = Activator.CreateInstance(type);

                data = data.Substring(index + 1);

                var attributes = data.Split(";".ToCharArray());
                foreach (string attribute in attributes)
                {
                    if (string.IsNullOrWhiteSpace(attribute))
                        continue;

                    var dataValueStrings = attribute.Split("=".ToCharArray());

                    PropertyInfo property = type.GetProperty(dataValueStrings[0]);
                    property.SetValue(value, Convert.ToInt32(dataValueStrings[1]), null);
                }

                return value;
            }
            throw new SerializationException("Could not DeSerialize type:" + objType);
        }

        private string GetCompleteTypeName(Type t)
        {
            //string fullType = t.FullName + ", " + t.Namespace + ", " + t.Assembly;
            string fullType = t.AssemblyQualifiedName;

            return fullType;
        }

        private object Read(string keyName)
        {
            // Opening the registry key
            RegistryKey rk = BaseRegKeyCurrentUser;
            // Open a subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey(SubKey, RegistryKeyPermissionCheck.ReadSubTree);
            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
                return null;
            try
            {
                return sk1.GetValue(keyName);
            }
            catch (Exception e)
            {
                LogWriter.LogError("Reading registry " + keyName, e);
                return null;
            }
        }

        private bool Write(RegistryDataTypes registryDataEntry)
        {
            if (registryDataEntry.Data == null) return false;

            try
            {
                RegistryKey rk = BaseRegKeyCurrentUser;
                RegistryKey sk1 = rk.OpenSubKey(SubKey, RegistryKeyPermissionCheck.ReadWriteSubTree) ?? rk.CreateSubKey(SubKey, RegistryKeyPermissionCheck.ReadWriteSubTree);
                sk1?.SetValue(registryDataEntry.KeyName, registryDataEntry.Data, registryDataEntry.DataType);

                return true;
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Writing registry" + registryDataEntry.KeyName, ex);
                return false;
            }
        }

        public bool DeleteKey(string keyName)
        {
            try
            {
                // Setting
                RegistryKey rk = BaseRegKeyCurrentUser;
                RegistryKey sk1 = rk.CreateSubKey(SubKey, RegistryKeyPermissionCheck.ReadWriteSubTree);
                // If the RegistrySubKey doesn't exists -> (true)
                sk1?.DeleteValue(keyName);

                return true;
            }
            catch (Exception e)
            {
                LogWriter.LogError("Deleting SubKey", e);
                return false;
            }
        }

        /// <summary>
        ///     To delete a sub key and any child.
        ///     input: void
        ///     output: true or false
        /// </summary>
        public bool DeleteSubKeyTree()
        {
            try
            {
                // Setting
                RegistryKey rk = BaseRegKeyCurrentUser;
                RegistryKey sk1 = rk.OpenSubKey(SubKey, RegistryKeyPermissionCheck.ReadWriteSubTree);
                // If the RegistryKey exists, I delete it
                if (sk1 != null)
                    rk.DeleteSubKeyTree(SubKey);

                return true;
            }
            catch (Exception e)
            {
                LogWriter.LogError("Deleting SubKey " + SubKey, e);
                return false;
            }
        }

        /// <summary>
        ///     Retrive the count of subkeys at the current key.
        ///     input: void
        ///     output: number of subkeys
        /// </summary>
        public int SubKeyCount()
        {
            try
            {
                // Setting
                RegistryKey rk = BaseRegKeyCurrentUser;
                RegistryKey sk1 = rk.OpenSubKey(SubKey, RegistryKeyPermissionCheck.ReadSubTree);
                // If the RegistryKey exists...
                if (sk1 != null)
                    return sk1.SubKeyCount;
                return 0;
            }
            catch (Exception e)
            {
                LogWriter.LogError("Retriving subkeys of " + SubKey, e);
                return 0;
            }
        }

        /// <summary>
        ///     Retrive the count of values in the key.
        ///     input: void
        ///     output: number of keys
        /// </summary>
        public int ValueCount()
        {
            try
            {
                // Setting
                RegistryKey rk = BaseRegKeyCurrentUser;
                RegistryKey sk1 = rk.OpenSubKey(SubKey, RegistryKeyPermissionCheck.ReadSubTree);
                // If the RegistryKey exists...
                if (sk1 != null)
                    return sk1.ValueCount;
                return 0;
            }
            catch (Exception e)
            {
                LogWriter.LogError("Retriving keys of " + SubKey, e);
                return 0;
            }
        }
    }
}