using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using CuplexApiCommon.BulkCopyModels;
using CuplexApiCommon.Converters;
using CuplexApiCommon.GeoIP.BoModels;
using DatabaseLib.BulkCopy;
using DatabaseLib.Linq;

namespace InternalServices.GeoIp
{
    public class GeoIPRepository
    {
        public GeoIPRepository()
        {
            ObjectMapper.Bind<GeoIPCity, GeoIPCityBo>((source, target) => target.LocationId = source.LocationId,
                (source, target) => target.CityName = source.CityName);
        }
        public GeoIPCountryBo GetGeoIpCountry(long ipNumber)
        {
            using (var db = ApiDataContext.Create())
            {
                GeoIPCountry ipLookup = db.GeoIPCountries.FirstOrDefault(x => ipNumber >= x.IPFrom && ipNumber <= x.IPTo);
                return ObjectMapper.Map<GeoIPCountry, GeoIPCountryBo>(ipLookup);
            }
        }

        public GeoIPCityBo GetGeoIpCity(long ipNumber)
        {
            GeoIPCity gerIpCity = null;
            using (var db = ApiDataContext.Create())
            {
                var lo = new DataLoadOptions();
                lo.LoadWith<GeoIPCityBlock>(c => c.GeoIPCity);
                db.LoadOptions = lo;

                GeoIPCityBlock geoBlock = db.GeoIPCityBlocks.Where(x => x.IPFrom <= ipNumber && x.IPTo >= ipNumber).Take(1).SingleOrDefault();
                if (geoBlock == null)
                    return null;

                gerIpCity = geoBlock.GeoIPCity;

            }
            var geoIpCityBo = ObjectMapper.Map<GeoIPCity, GeoIPCityBo>(gerIpCity);
            return geoIpCityBo;
        }

        public void ImportGeoIPCountryList(List<IBulkCopyItem> geopIpCountries, List<ColumnDefinition> columnDefinitions, Action<double> setProgress)
        {
            setProgress.Invoke(0);
            using (var db = ApiDataContext.Create())
            {
                db.ExecuteCommand("TRUNCATE TABLE GeoIPCountry");
            }

            BulkCopyCollection bulkCopyCollection = BulkCopyCollection.Create("GeoIPCountry", geopIpCountries.Count, columnDefinitions);
            bulkCopyCollection.BulkCopyItems.AddRange(geopIpCountries);

            BulkUploadToSql bulkUploadToSql = BulkUploadToSql.Load(bulkCopyCollection, setProgress);
            bulkUploadToSql.Flush();
            setProgress.Invoke(1);
        }

        public void ImportGeoIPCitiesList(List<IBulkCopyItem> geopIpBlocks, List<IBulkCopyItem> geoIpCities, List<ColumnDefinition> geoIpBlockDefinitions, List<ColumnDefinition> geoIpCityDefinitions, Action<double> setProgress)
        {
            using (var db = ApiDataContext.Create())
            {
                db.ExecuteCommand("Delete FROM GeoIPCityBlock");
                db.ExecuteCommand("Delete FROM GeoIPCity");
            }

            //Import geo ip city Blocks
            setProgress.Invoke(0);
            BulkCopyCollection bulkCopyCollection = BulkCopyCollection.Create("GeoIPCityBlock", geopIpBlocks.Count, geoIpBlockDefinitions);
            bulkCopyCollection.BulkCopyItems.AddRange(geopIpBlocks);

            BulkUploadToSql bulkUploadToSql = BulkUploadToSql.Load(bulkCopyCollection, setProgress);
            bulkUploadToSql.Flush();
            setProgress.Invoke(0);
            
            //Import geo ip city items
            bulkCopyCollection = BulkCopyCollection.Create("GeoIPCity", geoIpCities.Count, geoIpCityDefinitions);
            bulkCopyCollection.BulkCopyItems.AddRange(geoIpCities);

            bulkUploadToSql = BulkUploadToSql.Load(bulkCopyCollection, setProgress);
            bulkUploadToSql.Flush();
            setProgress.Invoke(1);
        }
    }
}