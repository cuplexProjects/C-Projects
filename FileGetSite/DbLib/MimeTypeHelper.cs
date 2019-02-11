using System.Reflection;
using System.Web;
using System;
public static class MimeExtensionHelper
{
    static object locker = new object();
    static object mimeMapping;
    static MethodInfo getMimeMappingMethodInfo;

    static MimeExtensionHelper()
    {
        Type mimeMappingType = Assembly.GetAssembly(typeof(HttpRuntime)).GetType("System.Web.MimeMapping");
        if (mimeMappingType == null)
            throw new SystemException("Couldnt find MimeMapping type");
        getMimeMappingMethodInfo = mimeMappingType.GetMethod("GetMimeMapping", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
        if (getMimeMappingMethodInfo == null)
            throw new SystemException("Couldnt find GetMimeMapping method");
        if (getMimeMappingMethodInfo.ReturnType != typeof(string))
            throw new SystemException("GetMimeMapping method has invalid return type");
        if (getMimeMappingMethodInfo.GetParameters().Length != 1 && getMimeMappingMethodInfo.GetParameters()[0].ParameterType != typeof(string))
            throw new SystemException("GetMimeMapping method has invalid parameters");
    }
    public static string GetMimeType(string filename)
    {
        lock (locker)
            return (string)getMimeMappingMethodInfo.Invoke(mimeMapping, new object[] { filename });
    }
}