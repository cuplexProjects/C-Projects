using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using GenerateOvpnFile.Misc;

namespace GenerateOvpnFile.Settings
{
    public class IniConfigFileManager
    {
        private const string ConfigFileSectionPattern = @"^\[\w+\]$";
        private const string ConfigSectionNamePattern = @"[^\w]";
        private const string ConfigFileItemPattern = @"^\w+=.+$";
        private IniConfigFile _iniFileData;
        public IniConfigFileManager()
        {
            _iniFileData = new IniConfigFile();
        }

        public bool LoadConfigFile(string path)
        {
            bool readSuccessfull = false;
            FileStream fs = null;
            try
            {
                if (!Converters.GetFileNameFromPath(path).EndsWith(".ini") || !File.Exists(path))
                    throw new ArgumentException("File does not exist!");

                fs = File.OpenRead(path);
                TextReader tr = new StreamReader(fs);

                Regex sectionMachRegex = new Regex(ConfigFileSectionPattern);
                Regex sectionCleanerRegex = new Regex(ConfigSectionNamePattern);
                Regex configItemPattern = new Regex(ConfigFileItemPattern);

                _iniFileData = new IniConfigFile();
                IniConfigFileSection configSection = new IniConfigFileSection();


                while (fs.Position <= fs.Length)
                {
                    string lineData = tr.ReadLine();

                    if (lineData == null || lineData.Length > 4096)
                        break;

                    if (sectionMachRegex.IsMatch(lineData))
                    {
                        string sectionName = sectionCleanerRegex.Replace(lineData, "");
                        if (!_iniFileData.ConfigSections.ContainsKey(sectionName))
                            _iniFileData.ConfigSections.Add(sectionName, new IniConfigFileSection());

                        configSection = _iniFileData.ConfigSections[sectionName];
                    }
                    else if (configSection != null && configItemPattern.IsMatch(lineData))
                    {
                        string[] confArr = lineData.Split('=');
                        configSection.ConfigItems[confArr[0].Trim()] = confArr[1].Trim();
                    }
                }
                fs.Close();
                readSuccessfull = true;

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(ex.Message);
            }
            finally
            {
                fs?.Close();
            }

            return readSuccessfull;
        }

        public void SaveConfigFile(string path)
        {
            try
            {
                if (!Converters.GetFileNameFromPath(path).EndsWith(".ini"))
                    throw new ArgumentException("Invalid filename. Filename must end with .ini");

                if (File.Exists(path))
                    File.Delete(path);

                FileStream fs = File.Create(path);
                TextWriter tw = new StreamWriter(fs);

                foreach (string rootItemName in _iniFileData.ConfigItems.AllKeys)
                {
                    tw.WriteLine(rootItemName + "=" + _iniFileData.ConfigItems[rootItemName]);
                }

                foreach (string sectionName in _iniFileData.ConfigSections.Keys)
                {
                    IniConfigFileSection section = _iniFileData.ConfigSections[sectionName];
                    tw.WriteLine("[" + sectionName + "]");

                    foreach (string itemName in section.ConfigItems.AllKeys)
                    {
                        tw.WriteLine(itemName + "=" + section.ConfigItems[itemName]);
                    }
                }
                tw.Flush();
                fs.Flush();

                fs.Close();

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(ex.Message);
            }
        }

        public IniConfigFile ConfigurationData
        {
            get { return _iniFileData; }
        }
    }

    [Serializable]
    public class IniConfigFile
    {
        public IniConfigFile()
        {
            ConfigSections = new Dictionary<string, IniConfigFileSection>();
            ConfigItems = new IniConfigItemCollection();
        }

        public Dictionary<string, IniConfigFileSection> ConfigSections { get; set; }
        public IniConfigItemCollection ConfigItems { get; set; }
    }

    public class IniConfigFileSection
    {
        public IniConfigFileSection()
        {
            ConfigItems = new IniConfigItemCollection();
        }
        public IniConfigItemCollection ConfigItems { get; private set; }
    }
}
