using System;
using System.Runtime.Remoting.Messaging;
using System.Security;

namespace RegKeyCreator.ApplicationKeys
{
    [SecuritySafeCritical]
    public static class RSAKeys
    {
        private static PublicKeysImplementation _publicKeys;
        private static PrivateKeysImplementation _privateKeys;
        
        public static RSAKeyFormatatBase PublicKeys => _publicKeys ?? (_publicKeys = new PublicKeysImplementation());

        public static RSAKeyFormatatBase PrivateKeys => _privateKeys ?? (_privateKeys = new PrivateKeysImplementation());

        private class PublicKeysImplementation : RSAKeyFormatatBase
        {
            [SecuritySafeCritical] private readonly string _pubString = @"
                -----BEGIN RSA PUBLIC KEY-----
                BgIAAACkAABSU0ExABAAAAEAAQDDRd/84GlMKzbcA7pu6bsL5qiQbgcXqKq29TP2AiSysLq14M1VLebdNRUsMPppsI+HdCP2T/VRrCBnKKAzjhBTiwVS6WP+/S/nL3NR8GMWGeq7K1uUg2T6tjubb9pEYqfQrIKLaUfyj20CmyZTSDst9huiPaRlz18qNM1zOSlro7wzhbVsmXJ0GFZFckA++P8W6qsY/74Gzf93L+dI8sGFx8frr5TPaIAWzaw92WGKUAEGkV41Q5Sjz8HHpGpvUZveacQn3Ev9F6Ne/AT1VXz8sPlhl36+yJUaxctR605j3KDq4ZsYAzrmf/vzp28gsOsiRAEop60oxCNxcYRtfyfJKzCLDi9oG85CN0PWFHriQoBKdxnx+6afTVCpfrxf5TTQVeaL+bh9Ffyv8eNP0RImqg/mxaEcOCZ4L4s7GaYJKtHPqkvgMVfSlMFc5PlaccAhsRWSwV/1I2zJO+ZDezvD2gzCWwEXHL5c0dgvolHeRbocCbS/wKYYXfGIawajf4Lum+c+iHQ0OmVnLO6ktbd+oBdBu2pEkjzIfaW076KA8rpDaNI2GhIKuHbg/xJbh48dNC9hIVQGyAdTQ+cKmRDoGhL1cr30FsMzMVUUQnGDjYSMvIP1+OB48SrR/qdbnnhLJAZhETl2KzR1cX6DRi1dIRRUrCeOaonRPi0T2kcJ0Q==
                -----END RSA PUBLIC KEY-----";

            [SecuritySafeCritical]
            public override byte[] GetBytes()
            {
                return Convert.FromBase64String(GetBase64Key());
            }

            [SecuritySafeCritical]
            public override string GetBase64Key()
            {
                return _pubString;
            }
        }

        [SecuritySafeCritical]
        private class PrivateKeysImplementation : RSAKeyFormatatBase
        {
            [SecuritySafeCritical] private readonly string _pkString = @"
            -----BEGIN RSA PRIVATE KEY-----
            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
            -----END RSA PRIVATE KEY-----";

            [SecuritySafeCritical]
            public override byte[] GetBytes()
            {
                return Convert.FromBase64String(GetBase64Key());
            }

            [SecuritySafeCritical]
            public override string GetBase64Key()
            {
                return _pkString;
            }
        }
    }
}