namespace DeleteDuplicateFiles.Delegates
{
    public class FileHashEventArgs
    {
        public string FileName;
        public long FileSize;

        public FileHashEventArgs()
        {
            
        }

        public FileHashEventArgs(string fileName, long fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
        }
    }
}
