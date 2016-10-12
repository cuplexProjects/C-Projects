using System;
using System.Runtime.Serialization;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract(Name = "BookmarkContainer", Namespace = "ImageView.DataContracts")]
    public class BookmarkContainer
    {
        public BookmarkContainer()
        {
        }

        public BookmarkContainer(BookmarkFolder rootFolder, string containerId, DateTime lastUpdate)
        {
            RootFolder = rootFolder;
            ContainerId = containerId;
            LastUpdate = lastUpdate;
        }

        [DataMember(Name = "RootFolder", Order = 1)]
        public BookmarkFolder RootFolder { get; set; }

        [DataMember(Name = "ContainerId", Order = 2)]
        public string ContainerId { get; set; }

        [DataMember(Name = "LastUpdate", Order = 3)]
        public DateTime LastUpdate { get; set; }
    }
}