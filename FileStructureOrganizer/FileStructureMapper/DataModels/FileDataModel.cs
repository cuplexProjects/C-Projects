using System;
using System.Runtime.Serialization;

namespace FileStructureMapper.DataModels
{
    [Serializable]
    [DataContract]
    public class FileDataModel
    {
        [DataMember]
        public string Name { get; set; }
    }
}