using System;
using System.Runtime.Serialization;

namespace FileStructureMapper.DataModels
{
    [Serializable]
    [DataContract]
    public class FolderDataModel
    {
        [DataMember]
        public string Name { get; set; }
    }
}