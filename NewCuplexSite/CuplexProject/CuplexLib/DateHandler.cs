using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CuplexLib
{
    public class DateHandler
    {
        public static string ToDateString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
        public static string ToTimeString(DateTime date)
        {
            return date.ToString("HH:mm:ss");
        }
        public static DateTime ParseDateString(string dateString)
        {
            return DateTime.ParseExact(dateString, "yyyy-MM-dd", null);
        }
        public static DateTime ParseTimeString(string dateString)
        {
            return DateTime.ParseExact(dateString, "HH:mm:ss", null);
        }
        public static bool TryParseDateString(string dateString, out DateTime date)
        {
            try
            {
                date = ParseDateString(dateString);
                return true;
            }
            catch 
            {
                date = DateTime.MinValue;
                return false; 
            }
        }
    }
}
