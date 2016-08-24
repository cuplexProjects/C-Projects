using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace SecureChat.Adapter.Webservice.Protocol.Server
{
    [ServiceContract]
    public interface IChatClientHostWebService
    {
        [OperationContract]
        [XmlSerializerFormat(Style = OperationFormatStyle.Rpc, Use = OperationFormatUse.Encoded)]
        bool CreateNewUser(CreateNewUserRequest userRequest);

        [OperationContract]
        [XmlSerializerFormat(Style = OperationFormatStyle.Rpc, Use = OperationFormatUse.Encoded)]
        bool PostMessage(ChatMessage message);

        [OperationContract]
        [XmlSerializerFormat(Style = OperationFormatStyle.Rpc, Use = OperationFormatUse.Encoded)]
        List<ChatMessage> GetAllMessages(string guid, string password, DateTime lastMessageTime);

        [OperationContract]
        [XmlSerializerFormat(Style = OperationFormatStyle.Rpc, Use = OperationFormatUse.Encoded)]
        List<ChatUserListItem> ListUsersInSearch(string nickname);

        [OperationContract]
        [XmlSerializerFormat(Style = OperationFormatStyle.Rpc, Use = OperationFormatUse.Encoded)]
        bool SendHeartbeat(string guid, string password);
    }
}
