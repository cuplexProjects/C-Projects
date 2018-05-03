using System;
using System.Runtime.Serialization;

namespace FileStructureMapper.DataModels
{
    [DataContract]
    public class ProfileDataModel
    {
        [DataMember]
        public string ProfileName { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string BasePath { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        public DateTime Modified { get; set; }
    }
}