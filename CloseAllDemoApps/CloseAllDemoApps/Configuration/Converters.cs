using System;

namespace CloseAllDemoApps.Configuration
{
    public static class Converters
    {
        public static string GetDirectoryNameFromPath(string path, bool trailingSlash = true)
        {
            int lastBackSlash = path.LastIndexOf('\\');

            if (lastBackSlash > 0)
            {
                if (trailingSlash)
                    lastBackSlash++;
                return path.Substring(0, lastBackSlash);
            }
            return path;
        }

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
