using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Voc_ScriptGen
{
    public class InternalResourceReader
    {
        public static Dictionary<string, string> GetLocalResources()
        {
            Dictionary<string, string> descriptionDictionary = new Dictionary<string, string>();
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Voc_ScriptGen.Properties.Resources.resources");
            ResourceReader res = new ResourceReader(stream);

            IDictionaryEnumerator dict = res.GetEnumerator();
            while (dict.MoveNext())
                descriptionDictionary.Add(dict.Key.ToString(), dict.Value.ToString());

            res.Close();
            return descriptionDictionary;
        }
    }
}