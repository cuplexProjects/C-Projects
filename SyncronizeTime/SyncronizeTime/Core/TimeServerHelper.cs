using System;
using System.Net.Sockets;
using SynchronizeTime.Common;
using SynchronizeTime.WinAPI;

namespace SynchronizeTime.Core
{
    public class TimeServerHelper
    {
        public string TimeServerUrl { get; set; }

        public bool SynchronizeTime()
        {
            TcpClient tcpClient = new TcpClient();
            long ticks = DateTime.Now.Ticks;

            try
            {
                tcpClient.Connect(TimeServerUrl, 37);
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog("Connection to time server failed - " + ex.Message);
                return false;
            }

            if (!tcpClient.Connected)
            {
                tcpClient.Close();
                return false;
            }

            NetworkStream ns = tcpClient.GetStream();
            byte[] bTime = new byte[8];
            int bytesRead = ns.Read(bTime, 0, 8);
            tcpClient.Close();
            if (bytesRead != 4)
                return false;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bTime);

            UInt32 u32Time = BitConverter.ToUInt32(bTime, 4);
            DateTime presentDate = GeneralConverters.GetDateTimeFromUInt(u32Time);

            UpdateSystemClock.SystemTime updatedTime = new UpdateSystemClock.SystemTime
            {
                Year = Convert.ToUInt16(presentDate.Year),
                Month = Convert.ToUInt16(presentDate.Month),
                Day = Convert.ToUInt16(presentDate.Day),
                DayOfWeek = Convert.ToUInt16(presentDate.DayOfWeek),
                Hour = Convert.ToUInt16(presentDate.Hour),
                Minute = Convert.ToUInt16(presentDate.Minute),
                Second = Convert.ToUInt16(presentDate.Second),
                Millisecond = 0
            };

            return UpdateSystemClock.Win32SetSystemTime(ref updatedTime);
        }
    }
}
