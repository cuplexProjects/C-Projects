using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;

namespace Voc_ScriptGen
{
    public class TemplateFileParser
    {
        public static Dictionary<string, TemplateFileItem> ParseFile(string fileName)
        {
            Dictionary<string, TemplateFileItem> itemList = new Dictionary<string, TemplateFileItem>();
            if (!File.Exists(fileName))
                throw new Exception("Input file does not exist");

            FileStream fs = File.Open(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            Regex rMatcher = new Regex(@"@\{[\w]+\}");
            
            while (!sr.EndOfStream)
            {
                string jsLine = sr.ReadLine();
                MatchCollection mc = rMatcher.Matches(jsLine);

                foreach (Match m in mc)
                {
                    if (itemList.ContainsKey(m.Value))
                    {
                        TemplateFileItem tfi = itemList[m.Value];
                        tfi.FilePositionList.Add(m.Index);
                    }
                    else
                        itemList.Add(m.Value, new TemplateFileItem(m.Value,m.Index));
                }
            }
            fs.Close();

            return itemList;
        }

        public static void GenerateOutput(string inputFileName,string outputFileName, Dictionary<string, TemplateFileItem> replacementDictionary)
        {
            FileStream fsInput = File.OpenRead(inputFileName);
            FileStream fsOutput = File.OpenWrite(outputFileName);
            StreamReader sr = new StreamReader(fsInput);
            StreamWriter streamWriter = new StreamWriter(fsOutput);
            string inputContent = sr.ReadToEnd();

            foreach (string key in replacementDictionary.Keys)
            {
                inputContent = inputContent.Replace(key, replacementDictionary[key].Replacement);
            }

            fsOutput.Position = 0;
            streamWriter.Write(inputContent);
            fsInput.Close();
            fsOutput.Close();
        }
    }

    public class TemplateFileItem
    {
        public string Variable;
        public string Replacement;
        public bool Enabled = true;
        private List<int> _filePositionList = null;

        public List<int> FilePositionList {
            get { return _filePositionList; }
        }

        public TemplateFileItem(string variable, int firstFilePos)
        {
            this.Variable = variable;
            _filePositionList = new List<int>();
            _filePositionList.Add(firstFilePos);
        }
        public TemplateFileItem(string variable)
        {
            this.Variable = variable;
            _filePositionList = new List<int>();
        }
    }
}