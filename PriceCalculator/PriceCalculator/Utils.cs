using System;

namespace PriceCalculator
{
    public static class Utils
    {
        public static string GetFileNameFromPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path can not be null or empty");

            int iPos = path.LastIndexOf('\\') + 1;
            if (iPos > 1 && iPos != path.Length)
                return path.Substring(iPos, path.Length - iPos);

            return path;
        }
    }
}
