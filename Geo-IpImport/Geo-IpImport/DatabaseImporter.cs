using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CuplexApiCommon;
using CuplexApiCommon.BulkCopyModels;
using CuplexApiCommon.GeoIP.BoModels;
using InternalServices.GeoIp;


namespace Geo_IpImport
{
    public class DatabaseImporter
    {
        public event Delegates.LogEventHandler OnError;
        public event EventHandler OnComplete;
        private readonly Delegates.SetProgress _progressCallback;
        private readonly Delegates.SetProgressText _progressText;
        private readonly StringBuilder _errorStringBuilder;
        private GeoIPRepository _geoIpRepository;
        private bool _cancelImport;
        
        public DatabaseImporter(Delegates.SetProgress progressCallback, Delegates.SetProgressText progressText)
        {
            _progressCallback = progressCallback;
            _progressText = progressText;
            _cancelImport = false;
            _errorStringBuilder= new StringBuilder();
        }

        public bool IsRunning { get; private set; }

        public void ImportGeoIPCountryData(string path, bool ignoreInvalidRows)
        {
            _cancelImport = false;
            FileStream fs = null;
            IsRunning = true;
            try
            {
                var files = Directory.EnumerateFiles(path, "GeoIPCountryWhois.csv").ToList();
                if (!files.Any())
                {
                    IsRunning = false;
                    if (OnComplete!=null)
                        OnComplete.Invoke(this,new EventArgs());
                    _progressText.Invoke("could not locate file 'GeoIPCountryWhois.csv', aborting.");
                    return;
                }

                List<IBulkCopyItem> geopIpCountries = new List<IBulkCopyItem>();
                string fileName = files.First();
                fs = File.OpenRead(fileName);
                StreamReader sr = new StreamReader(fs);
                _progressCallback.Invoke(0);
                long bytesRead = 0;
                while (!sr.EndOfStream)
                {
                    if (_cancelImport)
                        return;

                    string lineData = sr.ReadLine();

                    if (lineData == null)
                        break;

                    if (lineData.Length > 0 && lineData[0] == '#')
                        continue;

                    bytesRead += lineData.Length;
                    _progressCallback.Invoke((double)fs.Length / bytesRead);
                    MatchCollection mc = Regex.Matches(lineData, "\".*?\"");

                    try
                    {
                        if (mc.Count == 6)
                        {
                            var geoIpCountry = new GeoIPCountryBo()
                            {
                                IPAddressFrom = mc[0].Value.Replace("\"", ""),
                                IPAddressTo = mc[1].Value.Replace("\"", ""),
                                IPFrom = long.Parse(mc[2].Value.Replace("\"", "")),
                                IPTo = long.Parse(mc[3].Value.Replace("\"", "")),
                                CountryCode = mc[4].Value.Replace("\"", ""),
                                CountryName = mc[5].Value.Replace("\"", ""),
                            };
                            geopIpCountries.Add(new BulkCopyItemBo(geoIpCountry));
                        }
                    }
                    catch (Exception ex)
                    {
                        _progressText.Invoke("Exception while parsing GeoIPCountry row: " + ex.Message);
                        if (!ignoreInvalidRows)
                            return;
                    }
                }
                fs.Close();
                fs = null;

                _progressText.Invoke(string.Format("Finished parsing {0} rows from import file. Starting database import", geopIpCountries.Count));
                
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                _geoIpRepository = new GeoIPRepository();

                // GeoIPCountryBo column definition
                List<ColumnDefinition> geoIpCountryDefinitions= new List<ColumnDefinition>();
                geoIpCountryDefinitions.Add(new ColumnDefinition() { Index = 0, ColumnName = "IPFrom", ColumnDataType = typeof(long) });
                geoIpCountryDefinitions.Add(new ColumnDefinition() { Index = 1, ColumnName = "IPTo", ColumnDataType = typeof(long) });
                geoIpCountryDefinitions.Add(new ColumnDefinition() { Index = 2, ColumnName = "IPAddressFrom", ColumnDataType = typeof(string) });
                geoIpCountryDefinitions.Add(new ColumnDefinition() { Index = 3, ColumnName = "IPAddressTo", ColumnDataType = typeof(string) });
                geoIpCountryDefinitions.Add(new ColumnDefinition() { Index = 4, ColumnName = "CountryCode", ColumnDataType = typeof(string) });
                geoIpCountryDefinitions.Add(new ColumnDefinition() { Index = 5, ColumnName = "CountryName", ColumnDataType = typeof(string) });

                _geoIpRepository.ImportGeoIPCountryList(geopIpCountries, geoIpCountryDefinitions, d => _progressCallback(d));
                stopwatch.Stop();
                _progressText.Invoke("Database import completed after: " + stopwatch.Elapsed);

                if (OnComplete != null)
                    OnComplete.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                _errorStringBuilder.AppendLine(ex.Message);
                if (OnError != null)
                    OnError.Invoke(this, new ErrorEventArgs(ex));
            }
            finally
            {
                if (fs!=null)
                    fs.Close();
                IsRunning = false;
            }
        }
        public void ImportGeoIPCityData(string path, bool ignoreInvalidRows)
        {
            _cancelImport = false;
            IsRunning = true;
            FileStream fs = null;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                string blockImportFileName = Directory.EnumerateFiles(path, "GeoLiteCity-Blocks.csv").SingleOrDefault();

                if (blockImportFileName == null)
                {
                    IsRunning = false;
                    if (OnComplete != null)
                        OnComplete.Invoke(this, new EventArgs());
                    _progressText.Invoke("could not locate file 'GeoLiteCity-Blocks.csv', aborting.");
                    return;
                }

                string cityImportFileName = Directory.EnumerateFiles(path, "GeoLiteCity-Location.csv").SingleOrDefault();
                if (cityImportFileName == null)
                {
                    IsRunning = false;
                    if (OnComplete != null)
                        OnComplete.Invoke(this, new EventArgs());
                    _progressText.Invoke("could not locate file 'GeoLiteCity-Location.csv', aborting.");
                    return;
                }
                    

                var geopIPBlocks =  new ConcurrentBag<IBulkCopyItem>();
                var geoIPCities = new ConcurrentBag<IBulkCopyItem>();

                fs = File.OpenRead(blockImportFileName);
                var sr = new StreamReader(fs);
                _progressCallback.Invoke(0);
                stopwatch.Start();

                bool hasInvalidRow = false;
                string[] lineBuffer = new string[1000];
                
                while (!sr.EndOfStream)
                {
                    if (_cancelImport)
                    {
                        IsRunning = false;
                        if (OnComplete != null)
                            OnComplete.Invoke(this, new EventArgs());
                        return;
                    }

                    if (stopwatch.ElapsedMilliseconds >= 100)
                    {
                        _progressCallback.Invoke((double)fs.Position / fs.Length);
                        stopwatch.Restart();
                    }

                    int itemsRead = 0;
                    for (int i = 0; i < lineBuffer.Length; i++)
                    {
                        lineBuffer[i] = sr.ReadLine();
                        if (lineBuffer[i] == null)
                            break;
                        itemsRead++;
                    }
     
                    Parallel.For(0, itemsRead,
                        i =>
                        {
                            try
                            {
                                IBulkCopyItem geoBlockItem = ParseGeoIpBlock(lineBuffer[i]);
                                if (geoBlockItem != null)
                                    geopIPBlocks.Add(geoBlockItem);
                            }
                            catch (Exception ex)
                            {
                                hasInvalidRow = true;
                                _progressText.Invoke("Exception while parsing GeoIPBlock row: " + lineBuffer[i] + "\n" + ex.Message);
                            }
                        });

                    if (!ignoreInvalidRows && hasInvalidRow)
                        return;
                }
                
                fs.Close();
                fs = null;

                _progressText.Invoke(string.Format("Finished parsing {0} rows from GeoBlockIp import file.", geopIPBlocks.Count));

                fs = File.OpenRead(cityImportFileName);
                sr = new StreamReader(fs);
                _progressCallback.Invoke(0);

                stopwatch.Restart();
                while (!sr.EndOfStream)
                {
                    if (_cancelImport)
                    {
                        IsRunning = false;
                        if (OnComplete != null)
                            OnComplete.Invoke(this, new EventArgs());
                        return;
                    }

                    if (stopwatch.ElapsedMilliseconds >= 100)
                    {
                        _progressCallback.Invoke((double)fs.Position / fs.Length);
                        stopwatch.Restart();
                    }

                    int itemsRead = 0;
                    for (int i = 0; i < lineBuffer.Length; i++)
                    {
                        lineBuffer[i] = sr.ReadLine();
                        if (lineBuffer[i] == null)
                            break;
                        itemsRead++;
                    }

                    Parallel.For(0, itemsRead,
                        i =>
                        {
                            try
                            {
                                IBulkCopyItem geoIpCityItem = ParseGeoIpCity(lineBuffer[i]);
                                if (geoIpCityItem != null)
                                    geoIPCities.Add(geoIpCityItem);
                            }
                            catch (Exception ex)
                            {
                                hasInvalidRow = true;
                                _progressText.Invoke("Exception while parsing GeoIpCity row: " + lineBuffer[i] + "\n" + ex.Message);
                            }

                        });

                    if (!ignoreInvalidRows && hasInvalidRow)
                        return;
                }
                fs.Close();
                fs = null;
                stopwatch.Stop();
                _progressText.Invoke(string.Format("Finished parsing {0} rows from GeoIPCity import file. Starting database import", geoIPCities.Count));
                stopwatch.Restart();
                _geoIpRepository = new GeoIPRepository();
                _geoIpRepository.ImportGeoIPCitiesList(geopIPBlocks.ToList(), geoIPCities.ToList(), getGeoIpBlockColumnDefinitions(), getGeoIpCityColumnDefinitions(), d => _progressCallback(d));
                stopwatch.Stop();
                _progressText.Invoke("Database import completed after: "+stopwatch.Elapsed);
            }
            catch (Exception ex)
            {
                if (OnError != null)
                    OnError.Invoke(this, new ErrorEventArgs(ex));
            }
            finally
            {
                if (fs != null)
                    fs.Close();
                IsRunning = false;
            }

            if (OnComplete != null)
                OnComplete.Invoke(this, new EventArgs());
        }

        private IBulkCopyItem ParseGeoIpBlock(string lineData)
        {
            if (lineData == null||(lineData.Length > 0 && lineData[0] == '#'))
                return null;

            MatchCollection mc = Regex.Matches(lineData, "\".*?\"");
        
            if (mc.Count == 3)
            {
                GeoIPBlockBo getBlockBo = new GeoIPBlockBo()
                {
                    IPFrom = long.Parse(mc[0].Value.Replace("\"", "")),
                    IPTo = long.Parse(mc[1].Value.Replace("\"", "")),
                    LocationId = int.Parse(mc[2].Value.Replace("\"", "")),
                };

                return new BulkCopyItemBo(getBlockBo);
            }

            return null;
        }

        private IBulkCopyItem ParseGeoIpCity(string lineData)
        {
            string[] rc = Regex.Split(lineData, ",");

            if (lineData == null || (lineData.Length > 0 && lineData[0] == '#'))
                return null;

            if (rc.Length == 9)
            {
                string areaCode = rc[8].Replace("\"", "");
                if (string.IsNullOrWhiteSpace(areaCode))
                    areaCode = null;

                GeoIPCityBo geoIpCityBo = new GeoIPCityBo
                {
                    LocationId = int.Parse(rc[0].Replace("\"", "")),
                    CountryCode = rc[1].Replace("\"", ""),
                    RegionCode = rc[2].Replace("\"", ""),
                    CityName = rc[3].Replace("\"", ""),
                    PostalCode = rc[4].Replace("\"", ""),
                    Latitude = float.Parse(rc[5].Replace(".", ",")),
                    Longitude = float.Parse(rc[6].Replace(".", ",")),
                    MetroCode = rc[7].Length > 0 ? long.Parse(rc[7]) : (long?)null,
                    AreaCode = areaCode
                };
                return new BulkCopyItemBo(geoIpCityBo);
            }
            return null;
        }

        public async Task ImportGeoIPCountryDataAsync(string path, bool ignoreInvalidRows)
        {
            try
            {
                await Task.Run(() => ImportGeoIPCountryData(path, ignoreInvalidRows));
            }
            catch (Exception ex)
            {
                _errorStringBuilder.AppendLine(ex.Message);
                IsRunning = false;
            }
        }

        public async Task ImportGeoIPCityDataAsync(string path, bool ignoreInvalidRows)
        {
            try
            {
                await Task.Run(() => ImportGeoIPCityData(path, ignoreInvalidRows));
            }
            catch (Exception ex)
            {
                _errorStringBuilder.AppendLine(ex.Message);
                IsRunning = false;
            }
        }

        public void CancelImport()
        {
            _cancelImport = true;
        }

        private List<ColumnDefinition> getGeoIpBlockColumnDefinitions()
        {
            List<ColumnDefinition> columnDefinitions= new List<ColumnDefinition>();

            columnDefinitions.Add(new ColumnDefinition() { Index = 0, ColumnName = "LocationId", ColumnDataType = typeof(long) });
            columnDefinitions.Add(new ColumnDefinition() { Index = 1, ColumnName = "IPFrom", ColumnDataType = typeof(long) });
            columnDefinitions.Add(new ColumnDefinition() { Index = 2, ColumnName = "IPTo", ColumnDataType = typeof(long) });

            return columnDefinitions;
        }

        private List<ColumnDefinition> getGeoIpCityColumnDefinitions()
        {
            List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();

            columnDefinitions.Add(new ColumnDefinition() { Index = 0, ColumnName = "LocationId", ColumnDataType = typeof(long) });
            columnDefinitions.Add(new ColumnDefinition() { Index = 1, ColumnName = "CountryCode", ColumnDataType = typeof(string) });
            columnDefinitions.Add(new ColumnDefinition() { Index = 2, ColumnName = "RegionCode", ColumnDataType = typeof(string) });
            columnDefinitions.Add(new ColumnDefinition() { Index = 3, ColumnName = "CityName", ColumnDataType = typeof(string) });
            columnDefinitions.Add(new ColumnDefinition() { Index = 4, ColumnName = "PostalCode", ColumnDataType = typeof(string) });
            columnDefinitions.Add(new ColumnDefinition() { Index = 5, ColumnName = "Latitude", ColumnDataType = typeof(float) });
            columnDefinitions.Add(new ColumnDefinition() { Index = 6, ColumnName = "Longitude", ColumnDataType = typeof(float) });
            columnDefinitions.Add(new ColumnDefinition() { Index = 7, ColumnName = "MetroCode", ColumnDataType = typeof(long) });
            columnDefinitions.Add(new ColumnDefinition() { Index = 8, ColumnName = "AreaCode", ColumnDataType = typeof(string) });

            return columnDefinitions;
        }

        public string GetErrorMessage()
        {
            return _errorStringBuilder.ToString();
        }
    }
}
