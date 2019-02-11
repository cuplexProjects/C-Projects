using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;

namespace SetTimeAndDate
{
    public class TimeAndDate
    {
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;

            /// <summary>
            /// Convert form System.DateTime
            /// </summary>
            /// <param name="time"></param>
            public void FromDateTime(DateTime time)
            {
                wYear = (ushort)time.Year;
                wMonth = (ushort)time.Month;
                wDayOfWeek = (ushort)time.DayOfWeek;
                wDay = (ushort)time.Day;
                wHour = (ushort)time.Hour;
                wMinute = (ushort)time.Minute;
                wSecond = (ushort)time.Second;
                wMilliseconds = (ushort)time.Millisecond;
            }

            /// <summary>
            /// Convert to System.DateTime
            /// </summary>
            /// <returns></returns>
            public DateTime ToDateTime()
            {
                return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
            }
            /// <summary>
            /// STATIC: Convert to System.DateTime
            /// </summary>
            /// <param name="time"></param>
            /// <returns></returns>
            public static DateTime ToDateTime(SYSTEMTIME time)
            {
                return time.ToDateTime();
            }
        }

        public static DateTime GetAtomicTime(string hostName)
        {
            string ntpServer = hostName;

            // NTP message size - 16 bytes of the digest (RFC 2030)
            var ntpData = new byte[48];

            //Setting the Leap Indicator, Version Number and Mode values
            ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;

            //The UDP port number assigned to NTP is 123
            var ipEndPoint = new IPEndPoint(addresses[0], 123);
            //NTP uses UDP
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Connect(ipEndPoint);

            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();

            //Offset to get to the "Transmit Timestamp" field (time at which the reply 
            //departed the server for the client, in 64-bit timestamp format."
            const byte serverReplyTime = 40;

            //Get the seconds part
            ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

            //Get the seconds fraction
            ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

            //Convert From big-endian to little-endian
            intPart = SwapEndianness(intPart);
            fractPart = SwapEndianness(fractPart);

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

            //**UTC** time
            var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

            return networkDateTime;
        }

        // stackoverflow.com/a/3294698/162671
        static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                           ((x & 0x0000ff00) << 8) +
                           ((x & 0x00ff0000) >> 8) +
                           ((x & 0xff000000) >> 24));
        }

        //SetLocalTime C# Signature
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime( ref SYSTEMTIME Time );

    }
}
