using System;

namespace SecureChatServerModule.InternalServices
{
    public class KeyExchangeService : IDisposable
    {
        private static KeyExchangeService _instance;


        private KeyExchangeService()
        {

        }


        public void Dispose()
        {
            _instance = null;
        }

        public static KeyExchangeService Service
        {
            get { return _instance ?? (_instance = new KeyExchangeService()); }
        }
       
    }
}