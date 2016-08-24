using System.Net.Security;
using System.ServiceModel;

namespace SecureChat.Adapter.Webservice.Protocol.Client
{
    [ServiceContract(Namespace = "http://ClientWebSerice/WebService.wsdl", Name = "SecureChatClientWebService", ProtectionLevel = ProtectionLevel.EncryptAndSign, SessionMode = SessionMode.Allowed)]
    public interface IGuiWebService
    {
        [OperationContract]
        void MessageReceived(ClientChatMessage message);
    }
}
