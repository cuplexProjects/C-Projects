using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SecureChat.Adapter.Webservice.Protocol.Client
{
    [Serializable]
    [DataContract(Name = "ClientChatMessage")]
    public class ClientChatMessage
    {
        [DataMember(Name = "SenderGuid", Order = 1)]
        public string SenderGuid { get; set; }

        [DataMember(Name = "SenderData", Order = 2)]
        public string SenderData { get; set; }

        [DataMember(Name = "ServerHash", Order = 2)]
        public string ReceiverGuid { get; set; }
    }
}
