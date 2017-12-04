using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Apps.cuplex.se.Models;
using Serilog;

namespace Apps.cuplex.se.Utilities
{
    public class ApplicationListHelper
    {
        public List<AppViewModel> GenerateList()
        {
            var list = new List<AppViewModel>();


            try
            {
                var serverPath = HttpContext.Current.Server.MapPath("~");
                serverPath = serverPath + "Downloads";

                var directoryList = Directory.GetDirectories(serverPath);
                foreach (string directory in directoryList)
                {
                    var files = Directory.GetFiles(directory);
                    var fileinfoList = files.Select(x => new FileInfo(x)).ToList();

                    // get Latest msi
                    var selectedMsi = fileinfoList.OrderByDescending(x => x.CreationTime).FirstOrDefault(x => x.Name.EndsWith(".msi"));

                    if (selectedMsi == null)
                        continue;

                    string version = Regex.Match(selectedMsi.Name, @"[\d|\.].*", RegexOptions.IgnoreCase).Value.Replace(".msi","");
                    string appName = selectedMsi.Name.Replace(version, "").Replace(".msi", "").Replace("-","");
                    var request =  HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                                   HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";

                    string dirName = directory.Split('\\').Last();
                    var downloadUrl = $"{request}/Downloads/{dirName}/{selectedMsi.Name}";
                    list.Add(new AppViewModel
                    {
                        Name = appName,
                        Version = version,
                        DownloadUrl = new Uri(downloadUrl),
                        Filename = selectedMsi.Name,
                        FileDate = selectedMsi.CreationTime,
                        FileSize = selectedMsi.Length
                    });
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "ApplicationListHelper encountered an exception");
            }

            return list;
        }
    }
}