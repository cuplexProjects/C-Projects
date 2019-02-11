using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace CuplexLib.Linq
{
    class CSScript
    {
        [STAThread]
        static public void Main(string[] args)
        {
            string inputFile = args.Length >= 1 ? args[0] : "temp.dbml";
            string outputFile = args.Length >= 2 ? args[1] : "temp_modified.dbml";

            string result = File.ReadAllText(inputFile);

            Dictionary<string, string> enumDictionary = new Dictionary<string, string>();
            enumDictionary["DataType"] = "CuplexLib.SettingsDataType";

            XDocument dbml = XDocument.Parse(result);
            XNamespace ns = dbml.Document.Root.Name.Namespace;

            foreach (var column in dbml.Descendants(ns + "Column"))
            {
                XAttribute columnnameattr = column.Attribute("Name");
                string key = null;
                if (enumDictionary.ContainsKey((key = columnnameattr.Value)) ||
                    enumDictionary.ContainsKey((key = column.Parent.Attribute("Name").Value + "." + columnnameattr.Value)))
                {
                    string dataType = enumDictionary[key];
                    if (column.Attribute("CanBeNull").Value == "true")
                        dataType = "System.Nullable<" + dataType + ">";
                    column.Attribute("Type").SetValue(dataType);
                    Console.WriteLine(column.Parent.Attribute("Name").Value + "." + columnnameattr.Value + " => " + dataType);
                }
            }

            result = dbml.ToString();
            File.WriteAllText(outputFile, result);
        }
    }
}
