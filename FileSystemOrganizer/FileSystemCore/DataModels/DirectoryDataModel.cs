using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace FileSystemCore.DataModels
{
    [DataContract]
    [Serializable]
    public class DirectoryDataModel
    {
        private Guid _id;
        
        public DirectoryDataModel()
        {
            if (HashCode == 0)
            {
                HashCode = CalculateHashCode();
            }
        }

        [DataMember(Name = "DirectoryId", Order = 0, IsRequired = true)]
        public Guid DirectoryId
        {
            get
            {
                if (_id == Guid.Empty)
                {
                    _id = Guid.NewGuid();
                }

                return _id;
            }
            protected set => _id = value;
        }

        [DataMember(Name = "Name", Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "FullPath", Order = 2)]
        public string FullPath { get; set; }

        [DataMember(Name = "Created", Order = 3)]
        public DateTime Created { get; set; }

        [DataMember(Name = "NormalUserAccesss", Order = 4)]
        public bool NormalUserAccesss { get; set; }

        [DataMember(Name = "ParentDirectory", Order = 5)]
        public Guid ParentDirectory { get; set; }

        [DataMember(Name = "Attributes", Order = 6)]
        public FileAttributes Attributes { get; set; }

        [DataMember(Name = "HashCode", Order = 7)]
        public int HashCode { get; protected set; }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            // Stays unique once the object in instanciated the first time
            // Serialization should not change the value
            return HashCode;
        }

        public override string ToString()
        {
            return FullPath;
        }

        private int CalculateHashCode()
        {
            int hashCode = 0x1 << 8;
            checked
            {
                hashCode = DirectoryId.ToByteArray().Aggregate(hashCode, (current, b) => current + b);
            }

            return hashCode;
        }
    }
}