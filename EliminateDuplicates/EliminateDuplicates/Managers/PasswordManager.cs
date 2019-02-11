using System;
using System.Security;
using System.Text;
using GeneralToolkitLib.Storage.Memory;
using JetBrains.Annotations;

namespace DeleteDuplicateFiles.Managers
{
    /// <summary>
    /// Password Manager
    /// </summary>
    /// <seealso cref="DeleteDuplicateFiles.Managers.ManagerBase" />
    /// <seealso cref="System.IDisposable" />
    [UsedImplicitly]
    public class PasswordManager : ManagerBase, IDisposable
    {
        /// <summary>
        /// The base code
        /// </summary>
        private const string BaseCode = "8LVzDWXmrVM9NPjRk5eaTY6ELxAyk6CPutsyyF4ZtGj8DNnxXfWdTJXqdEhZ7EOgzdFhQ00LYzhbrkPYddjZrNpcbQ1R3hUnwEYXmgDF93ubC4WOwWmx2PsdwOR5pHpO";
        /// <summary>
        /// The salt value1
        /// </summary>
        private const string SaltValue1 = "nW5Tklo4j9RTRAzXNdpnoZVDrFJ9bDibJXQxozGYF9vaQzuwoNdzSBgRajXWdJWJ";
        /// <summary>
        /// The salt value2
        /// </summary>
        private const string SaltValue2 = "bw1nmENCbb0udeopOVUDDvFwhMeJlobSEs6U48M4eAvpL4o2csGIkNE4sborLcOB";
        
        /// <summary>
        /// The password storage
        /// </summary>
        private readonly PasswordStorage _passwordStorage;

        /// <summary>
        /// 
        /// </summary>
        public enum PasswordTypes
        {
            /// <summary>
            /// The hash file
            /// </summary>
            HashFile,
            /// <summary>
            /// The profile
            /// </summary>
            Profile,
            /// <summary>
            /// The settings
            /// </summary>
            Settings,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordManager"/> class.
        /// </summary>
        [SecurityCritical]
        public PasswordManager()
        {
            _passwordStorage = new PasswordStorage();
        }

        /// <summary>
        /// Initializes the default password.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        [SecurityCritical]
        public void InitDefaultPwd(string accessToken)
        {
            var initBytes = Encoding.UTF8.GetBytes(string.Concat(SaltValue1, BaseCode, SaltValue2));
            var defaultPwd = GeneralToolkitLib.Hashing.SHA256.GetSHA256HashAsHexString(initBytes);
            _passwordStorage.Set(accessToken, defaultPwd);
        }

        /// <summary>
        /// Adds the password.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="password">The password.</param>
        [SecurityCritical]
        public void SetPassword(string key, string password) => _passwordStorage.Set(key, password);

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [SecurityCritical]
        public string GetPassword(string key) => _passwordStorage.Get(key);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _passwordStorage?.Dispose();
        }
    }
}
