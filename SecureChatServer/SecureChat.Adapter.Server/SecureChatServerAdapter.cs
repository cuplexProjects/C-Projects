using SecureChat.Adapter.Server.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureChat.Common.TinyMessenger;

namespace SecureChat.Adapter.Server
{
    public class SecureChatServerAdapter 
    {
        private readonly ITinyMessengerHub _msgHub;
        public Action Shutdown { get; set; }
        private bool _isReady;

        public SecureChatServerAdapter()
        {
            
        }


        public void Init()
        {
            var host = new WebServiceInterfaceFeature { Adapter = this };
            host.Setup();
            _isReady = true;

            Shutdown += host.Stop;
        }

        public bool IsReady()
        {
            return _isReady;
        }

        public void Stop()
        {
            if (Shutdown != null)
                Shutdown();
        }
    }
}
