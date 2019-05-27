using System;
using System.Drawing.Text;
using System.Text;

namespace RegKeyCreator.ApplicationKeys
{
    public abstract class RSAKeyFormatatBase
    {
        public abstract byte[] GetBytes();

        public abstract string GetBase64Key();

        public string GetKeyString()
        {
            return GetBase64Key();
        }
    }
}