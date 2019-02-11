using System;

namespace PrefetchFile
{
    public class FileReadProgress
    {
        public long BytesRead { get; set; }
        public long TotalBytes { get; set; }
        public bool Completed { get; set; }

        public int PercentComplete => TotalBytes > 0 ? Convert.ToInt32(((double) BytesRead/TotalBytes)*100) : 0;
    }
}