using System;
using Common.DtoModels;
using DatabaseLib.SecureChat;
using GeneralToolkitLib.Log;
using SecureChatServerModule.Exceptions;

namespace SecureChatServerModule.InternalServices.DHServiceImplementation
{
    public class DiffieHellmanService : IEngineService
    {
        private static DiffieHellmanService _instaService;
        private bool _running;
        private bool _initializing;
        private readonly DBServerSettingsManager _dbServerSettingsManager;
        private DiffieHellmanConfigurationDto _diffieHellmanConfiguration;


        private DiffieHellmanService()
        {
            _dbServerSettingsManager = new DBServerSettingsManager();
        }

        public async void Initialize()
        {
            try
            {
                _initializing = true;
                _diffieHellmanConfiguration = _dbServerSettingsManager.GetDiffieHellmanConfiguration();
                _running = true;
            }
            catch(Exception ex)
            {
                LogWriter.WriteLog(ex.Message);

                if(ex is ServerConfigMissingException)
                {
                    ServerConfigMissingException serverConfigException = ex as ServerConfigMissingException;
                    LogWriter.WriteLog("Creating new Configuration");
                }
            }

            if(!_running)
            {
                //_running = await generateServerTaskDHParams();
            }
                
        }

        //private async Task<bool> generateServerTaskDHParams()
        //{
        //    using (ECDiffieHellmanCng dbCng = new ECDiffieHellmanCng(521))
        //    {
        //        dbCng.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
        //        dbCng.HashAlgorithm = CngAlgorithm.Sha256;
        //        byte[] serverPublicKey = dbCng.PublicKey.ToByteArray();

        //        _diffieHellmanConfiguration = new DiffieHellmanConfigurationDto();

        //        CngKey k = CngKey.Import(bob.bobPublicKey, CngKeyBlobFormat.EccPublicBlob);
        //        byte[] aliceKey = dbCng.DeriveKeyMaterial(CngKey.Import(bob.bobPublicKey, CngKeyBlobFormat.EccPublicBlob));
        //        byte[] encryptedMessage = null;
        //        byte[] iv = null;
        //        Send(aliceKey, "Secret message", out encryptedMessage, out iv);
        //        bob.Receive(encryptedMessage, iv);
        //    }

        //    _initializing = false;
        //    return true;
        //}


        public static DiffieHellmanService Service
        {
            get { return _instaService ?? (_instaService = new DiffieHellmanService()); }
        }

        public void Dispose()
        {
            this.Stop();
        }

        public void Start()
        {
            if (!_running && !_initializing)
            {
                Initialize();
            }
        }

        public void Stop()
        {
         
        }

        public bool IsRunning {
            get { return _running; }
        }
    }
}