using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CLinq = CuplexLib.Linq;
using System.Web;

namespace CuplexLib
{
    [Serializable]
    public class Settings
    {
        public const int CacheTime = 300;  //seconds

        public string KeyType { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public SettingsDataType? DataType { get; set; }

        public static Settings GetOne(string keyType)
        {
            Settings s = new Settings();
            using (var db = CLinq.DataContext.Create())
            {
                var setting = db.Settings.Where(x => x.KeyType == keyType).Take(1).SingleOrDefault();
                if (setting != null)
                {
                    s.Description = setting.Description;
                    s.KeyType = setting.KeyType;
                    s.Value = setting.Value;
                    s.DataType = setting.DataType;
                }
            }

            return s;
        }

        public static Settings GetOneFromCache(string keyType)
        {
            Settings s = HttpRuntime.Cache["DBSetting:"+keyType] as Settings;
            if (s == null)
            {
                s = GetOne(keyType);
                HttpRuntime.Cache.Insert("DBSetting:" + keyType, s, null, DateTime.Now.AddMinutes(CacheTime), System.Web.Caching.Cache.NoSlidingExpiration);
            }

            return s;
        }
        public static void ClearCache(List<string> keyList)
        {
            foreach (string key in keyList)
                HttpRuntime.Cache.Remove("DBSetting:" + key);
        }
        public static List<string> GetKeyTypeList()
        {
            using (var db = CLinq.DataContext.Create())
            {
                return db.Settings.Select(s => s.KeyType).ToList();
            }
        }
        public static bool IsCorrectDataType(string value, SettingsDataType? dataType)
        {
            if (dataType == null)
                return true;
            switch (dataType.Value)
            {
                case SettingsDataType.Boolean:
                    if (string.IsNullOrEmpty(value))
                        return false;
                    else return value == "0" || value == "1";
                    break;
                case SettingsDataType.Integer:
                    if (string.IsNullOrEmpty(value))
                        return false;
                    if (value.Length > 10)
                        return false;
                    return Regex.IsMatch(value, "^([-]|[0-9])[0-9]*$");
                    break;
                case SettingsDataType.String:
                    if (value == null)
                        return false;
                    else
                        return true;
                    break;
                default:
                    return false;
            }
        }
    }
    public enum SettingsDataType
    {
        Integer = 1,
        String = 2,
        Boolean = 3
    }
}