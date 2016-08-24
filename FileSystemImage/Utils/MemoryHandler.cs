using System;
using GeneralToolkitLib.Log;

namespace FileSystemImage.Utils
{
    static class MemoryHandler
    {
        public static void RunGarbageCollect()
        {
            GC.Collect(0, GCCollectionMode.Forced);
            long memAlloc = GC.GetTotalMemory(true);
#if DEBUG
            LogWriter.LogMessage("Current Allocated Memory Is: " + memAlloc/1024 + " kb", LogWriter.LogLevel.Debug);
#endif
        }
    }
}
