using System;
using System.Linq;
using System.Collections.Generic;

namespace ImageView
{
    [Serializable]
    public class ImageViewApplicationSettings
    {
        public ImageViewApplicationSettings()
        {
            AlwaysOntop = true;
            AutoRandomizeCollection = true;
            LastUsedSearchPaths = new List<string>(); 
        }

        public void RemoveDuplicateEntriesWithIgnoreCase()
        {
            Stack<string> deleteStack = new Stack<string>();
            foreach(string searchPath in LastUsedSearchPaths)
            {
                if (LastUsedSearchPaths.Any(s => s.ToLower() == searchPath))
                    deleteStack.Push(searchPath);                    
            }

            while (deleteStack.Count > 0)
                LastUsedSearchPaths.Remove(deleteStack.Pop());            
        }

        public bool AlwaysOntop { get; set; }
        public bool AutoRandomizeCollection { get; set; }
        public List<string> LastUsedSearchPaths { get;  set; }
    }
}
