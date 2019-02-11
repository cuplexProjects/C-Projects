using System;
using System.Runtime.Serialization;

namespace SecureChatSharedComponents.DataModels
{
    [Serializable]
    [DataContract(Name = "SecureChatUserInfo")]
    public class SecureChatUserInfo
    {
        [DataMember(Name = "FirstName", Order = 1)]
        public string FirstName { get; set; }

        [DataMember(Name = "LastName", Order = 2)]
        public string LastName { get; set; }

        [DataMember(Name = "DateOfBirth", Order = 3)]
        public DateTime DateOfBirth { get; set; }

        [DataMember(Name = "City", Order = 4)]
        public string City { get; set; }

        [DataMember(Name = "ZipCode", Order = 5)]
        public string ZipCode { get; set; }

        [DataMember(Name = "Country", Order = 6)]
        public string Country { get; set; }

        [DataMember(Name = "PhoneNumber", Order = 7)]
        public string PhoneNumber { get; set; }
        
        [DataMember(Name = "Email", Order = 8)]
        public string Email { get; set; }

        [DataMember(Name = "EncodedAvatar", Order = 9)]
        public byte[] AvatarEncoded { get; set; }
    }
}
