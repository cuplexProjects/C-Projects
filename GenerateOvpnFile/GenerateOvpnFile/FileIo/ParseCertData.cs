using System.IO;

namespace GenerateOvpnFile.FileIo
{
    static class ParseCertData
    {
        public static string ReadFileData(string filename)
        {
            string fileData = null;
            FileStream fileStream = null;
            try
            {
                if(!File.Exists(filename))
                    return null;

                fileStream = File.Open(filename, FileMode.Open);
                TextReader tr = new StreamReader(fileStream);
                
                if(filename.EndsWith(".key"))
                {
                    fileData = tr.ReadToEnd();
                    fileData = fileData.Trim();
                    return fileData;
                }
                
                while(fileStream.CanRead && fileStream.Position != fileStream.Length)
                {
                    string lineData = tr.ReadLine();
                    if(lineData != "-----BEGIN CERTIFICATE-----") continue;
                    fileData = lineData + "\r\n" + tr.ReadToEnd();
                    fileData = fileData.Trim();
                    break;
                }
            }
            finally
            {
                if(fileStream != null)
                    fileStream.Close();
            }

            return fileData;
        }
    }
}
