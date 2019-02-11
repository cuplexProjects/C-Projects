using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using SecureChat.Adapter.Webservice.Protocol.Server;

namespace SecureChat.Adapter.Server.Features
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class WebServiceInterfaceFeature : IHostFeature, IChatClientHostWebService
    {
        private ServiceHost _serviceHost;
        public SecureChatServerAdapter Adapter { private get; set; }
        
        public void Setup()
        {
            var serviceAddress = new Uri("http://0.0.0.0:8090/SecureChatHost");
            _serviceHost = new ServiceHost(this, serviceAddress);

            var serviceMetaDataBehaviour = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                MetadataExporter = { PolicyVersion = PolicyVersion.Policy15 }
            };
            _serviceHost.Description.Behaviors.Add(serviceMetaDataBehaviour);
            _serviceHost.Open();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public bool CreateNewUser(CreateNewUserRequest userRequest)
        {
            throw new NotImplementedException();
        }

        public bool PostMessage(ChatMessage message)
        {
            throw new NotImplementedException();
        }

        public List<ChatMessage> GetAllMessages(string guid, string password, DateTime lastMessageTime)
        {
            throw new NotImplementedException();
        }

        public List<ChatUserListItem> ListUsersInSearch(string nickname)
        {
            throw new NotImplementedException();
        }

        public bool SendHeartbeat(string guid, string password)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
