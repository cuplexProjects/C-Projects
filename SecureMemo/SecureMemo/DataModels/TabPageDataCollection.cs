using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SecureMemo.DataModels
{
    [Serializable]
    [DataContract]
    public class TabPageDataCollection
    {
        public TabPageDataCollection()
        {
            TabPageDictionary = new Dictionary<int, TabPageData>();
        }

        [DataMember(Name = "TabPageDictionary", Order = 1)]
        public Dictionary<int, TabPageData> TabPageDictionary { get; set; }

        [DataMember(Name = "ActiveTabIndex", Order = 2)]
        public int ActiveTabIndex { get; set; }

        public static TabPageDataCollection CreateNewPageDataCollection(int numberOfPages)
        {
            var pageDataCollection = new TabPageDataCollection {TabPageDictionary = new Dictionary<int, TabPageData>()};

            for (int i = 0; i < numberOfPages; i++)
            {
                pageDataCollection.TabPageDictionary.Add(i, new TabPageData {PageIndex = i, TabPageLabel = "Page" + (i + 1)});
                pageDataCollection.TabPageDictionary.Values.Last().GenerateUniqueIdIfNoneExists();
            }

            return pageDataCollection;
        }
    }
}