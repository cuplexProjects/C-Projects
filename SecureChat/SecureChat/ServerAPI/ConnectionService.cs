using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using GeneralToolkitLib.Encryption;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using SecureChat.DataModels;
using SecureChat.Settings;
using SecureChatSharedComponents.ApiDtos;
using SecureChatSharedComponents.DataModels;

namespace SecureChat.ServerAPI
{
    public class ConnectionService : IDisposable
    {
        private static ConnectionService _instance;
        private readonly JsonSerializer _jsonSerializer;

        private ConnectionService()
        {
            _jsonSerializer = new JsonSerializer();
            UpdateSettins();
        }

        public static ConnectionService Instance {
            get { return _instance ?? (_instance = new ConnectionService()); }
        }

        public void UpdateSettins()
        {
            HostNameURL = ApplicationSettingsService.SettingsService.AppSettings.APIHostName;
        }

        public string HostNameURL { get; private set; }

        public async Task<SecureChatUser> RegisterNewUser(SecureChatCreateUserRequest userRequest, RSAKeySetIdentity keySetIdentity)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HostNameURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = await client.PostAsJsonAsync("api/securechat/users", userRequest);
                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.
                    string responceMessage = await response.Content.ReadAsStringAsync();

                    StringReader sr = new StringReader(responceMessage);
                    JsonReader jsonReader = new JsonTextReader(sr);
                    CreateNewUser createNewUserServerResponce = _jsonSerializer.Deserialize<CreateNewUser>(jsonReader);

                    

                    RSA_AsymetricEncryption rsaAsymetricEncryption= new RSA_AsymetricEncryption();
                    RSAParameters rsaParameters = rsaAsymetricEncryption.ParseRSAPublicKeyOnlyInfo(keySetIdentity);

                    string aesKey= rsaAsymetricEncryption.DecryptObjectUsingRSA(createNewUserServerResponce.EncodedAESKey, rsaParameters);
                     


                    // Todo decrypt incoming data using AES
                    SecureChatUser secureChatUser= new SecureChatUser();

                    
                    //register public key

                    //PublicKeyData publicKeyData = new PublicKeyData();
                    //publicKeyData.GUID = userKeySet.RSA_GUID;
                    //publicKeyData.PublicKey = userKeySet.RSA_PublicKey;
                    //response = await client.PostAsJsonAsync("api/securechat/publickeys", publicKeyData);


                    if (response.IsSuccessStatusCode)
                        return secureChatUser;
                }

            }

            return null;
        }

        public void Disconnect()
        {

        }

        public void Dispose()
        {
            _instance = null;
            GC.Collect(0, GCCollectionMode.Forced);
        }
    }
}
