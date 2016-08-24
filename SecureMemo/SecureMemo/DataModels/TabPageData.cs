using System;
using System.Runtime.Serialization;

namespace SecureMemo.DataModels
{
    [Serializable]
    [DataContract]
    public class TabPageData
    {
        [DataMember(Name = "PageIndex", Order = 1)]
        public int PageIndex { get; set; }

        [DataMember(Name = "PageLabel", Order = 2)]
        public string TabPageLabel { get; set; }

        [DataMember(Name = "TabPageText", Order = 3)]
        public string TabPageText { get; set; }

        [DataMember(Name = "UniqueId", Order = 4)]
        public string UniqueId { get; set; }

        public bool GenerateUniqueIdIfNoneExists()
        {
            if (UniqueId != null) return false;
            UniqueId = Guid.NewGuid().ToString();
            return true;
        }
    }
}