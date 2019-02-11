using System.IO;
using System.Text;
using GenerateOvpnFile.Models;

namespace GenerateOvpnFile.FileIo
{
    static class GenerateVPNConfig
    {
        public static void SaveConfigFile(string path, OpenVPNConnection ovpnConn)
        {
            FileStream fileStream = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("client");
                sb.AppendLine("dev " + ovpnConn.InterfaceType.ToString().ToLower());
                sb.AppendLine("proto " + ovpnConn.Protocol.ToString().ToLower());
                sb.AppendLine("remote " + ovpnConn.Hostname + " " + ovpnConn.ServerPort);
                sb.AppendLine("cipher " + ovpnConn.Cipher);

                sb.AppendLine("nobind");
                sb.AppendLine("persist-key");
                sb.AppendLine("persist-tun");
                sb.AppendLine("resolv-retry infinite");
                sb.AppendLine("verb 3");

                if(ovpnConn.Compression==ConfigEnums.Compression.None)
                    sb.AppendLine("comp-lzo no");
                else if(ovpnConn.Compression == ConfigEnums.Compression.Adaptive)
                    sb.AppendLine("comp-lzo adaptive");
                else
                    sb.AppendLine("comp-lzo yes");

                sb.AppendLine("keepalive 15 60");
                sb.AppendLine("ns-cert-type server");

                if(ovpnConn.TlsAuth == ConfigEnums.TLS_Auth.Enabled)
                    sb.AppendLine("remote-cert-tls server");

                sb.AppendLine("<ca>");
                sb.AppendLine(ovpnConn.CaCertFileData);
                sb.AppendLine("</ca>");

                sb.AppendLine("<cert>");
                sb.AppendLine(ovpnConn.UserCertFileData);
                sb.AppendLine("</cert>");

                sb.AppendLine("<key>");
                sb.AppendLine(ovpnConn.UserCertPrivateKeyData);
                sb.AppendLine("</key>");

                if(ovpnConn.TlsAuth == ConfigEnums.TLS_Auth.Enabled)
                {
                    sb.AppendLine("<tls-auth>");
                    sb.AppendLine(ovpnConn.TaKeyData);
                    sb.AppendLine("</tls-auth>");
                }

                fileStream = File.Create(path);
                TextWriter tr = new StreamWriter(fileStream);
                tr.Write(sb.ToString());
                tr.Flush();
                fileStream.Flush();
            }
            finally
            {
                if(fileStream != null)
                    fileStream.Close();
            }
        }
    }
}
