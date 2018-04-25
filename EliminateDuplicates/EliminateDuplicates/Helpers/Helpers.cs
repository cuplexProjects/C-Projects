using System;
using System.Linq;
using System.Reflection;

namespace DeleteDuplicateFiles.Helpers
{
    /// <summary>
    /// GetBasicData Helper
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Generates the hashcode.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static int GenerateHashcode(object instance)
        {
            PropertyInfo[] propertyInfos = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty);
            int[] propHashes= (from propertyInfo in propertyInfos select propertyInfo.Name.GetHashCode()).ToArray();

            Guid hashGuid = Guid.NewGuid();
            int hashcode = hashGuid.GetHashCode();

            unchecked
            {
                hashcode = propHashes.Aggregate(hashGuid.ToByteArray().Aggregate(hashcode, (current, b) => (current * 397) ^ b), (current, propHash) => (current * 397) ^ propHash);
            } 

            return hashcode;
        }
    }
}
