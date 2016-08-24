using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IpLocationUpdate
{
    public class IpLocation
    {
        public long IpFrom { get; set; }
        public long IpTo { get; set; }
        public string Registry { get; set; }
        public long Assigned { get; set; }
        public string Ctry { get; set; }
        public string Cntry { get; set; }
        public string Country { get; set; }


        public static List<IpLocation> ParseInputFile(string fileName)
        {
            List<IpLocation> retList = new List<IpLocation>();

            try
            {
                FileStream stream = File.OpenRead(fileName);
                StreamReader sr = new StreamReader(stream);

                string rowData;
                while (!sr.EndOfStream)
                {
                    rowData = sr.ReadLine();
                    if (rowData.Length > 0 && rowData[0] != '#')
                    {
                        string[] rowDataArr = parseToArr(rowData);
                        if (rowDataArr.Length == 7)
                        {
                            try
                            {
                                IpLocation ipLoc = new IpLocation();
                                ipLoc.IpFrom = long.Parse(rowDataArr[0]);
                                ipLoc.IpTo = long.Parse(rowDataArr[1]);
                                ipLoc.Registry = rowDataArr[2];
                                ipLoc.Assigned = long.Parse(rowDataArr[3]);
                                ipLoc.Ctry = rowDataArr[4];
                                ipLoc.Cntry = rowDataArr[5];
                                ipLoc.Country = rowDataArr[6];

                                retList.Add(ipLoc);
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }
            return retList;
        }
        private static string[] parseToArr(string data)
        {
            string[] retVal = data.Split(",\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < retVal.Length; i++ )
                retVal[i] = retVal[i].TrimEnd("\"".ToCharArray());

            return retVal;
        }
    }    
}