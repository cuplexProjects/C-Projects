using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract(Name = "BookmarkContainer", Namespace = "ImageView.DataContracts")]
    [ProtoContract(AsReferenceDefault = true, DataMemberOffset = 0, Name = "BookmarkContainer", UseProtoMembersOnly = true)]
    public class BookmarkContainer
    {
        protected BookmarkContainer()
        {
            
        }

        public BookmarkContainer(BookmarkFolder rootFolder, string containerId, DateTime lastUpdate)
        {
            RootFolder = rootFolder;
            ContainerId = containerId;
            LastUpdate = lastUpdate;
        }

        [DataMember(Name = "RootFolder", Order = 1)]
        [ProtoMember(1,AsReference = true,IsRequired = true,Name = "RootFolder")]
        public BookmarkFolder RootFolder { get; protected set; }

        [DataMember(Name = "ContainerId", Order = 2)]
        [ProtoMember(2, AsReference = true, IsRequired = true, Name = "ContainerId")]
        public string ContainerId { get; protected set; }

        [DataMember(Name = "LastUpdate", Order = 3)]
        [ProtoMember(3, AsReference = true, IsRequired = true, Name = "LastUpdate")]
        public DateTime LastUpdate { get; protected set; }
    }
}