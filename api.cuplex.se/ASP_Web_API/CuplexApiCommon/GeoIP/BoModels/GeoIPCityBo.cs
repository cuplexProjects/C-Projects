using System.Collections.Generic;
using CuplexApiCommon.BulkCopyModels;

namespace CuplexApiCommon.GeoIP.BoModels
{
    public class GeoIPCityBo : IBulkCopyItem
    {
        public long LocationId { get; set; }
        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public long? MetroCode { get; set; }
        public string AreaCode { get; set; }
        public IEnumerable<object> RowObjects { get; set; }

        protected bool Equals(GeoIPCityBo other)
        {
            return LocationId == other.LocationId && string.Equals(CountryCode, other.CountryCode) &&
                   string.Equals(RegionCode, other.RegionCode) &&
                   string.Equals(CityName, other.CityName) &&
                   string.Equals(PostalCode, other.PostalCode) &&
                   Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude) &&
                   MetroCode == other.MetroCode &&
                   string.Equals(AreaCode, other.AreaCode);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = LocationId.GetHashCode();
                hashCode = (hashCode*397) ^ (CountryCode != null ? CountryCode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (RegionCode != null ? RegionCode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (CityName != null ? CityName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (PostalCode != null ? PostalCode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Latitude.GetHashCode();
                hashCode = (hashCode*397) ^ Longitude.GetHashCode();
                hashCode = (hashCode*397) ^ MetroCode.GetHashCode();
                hashCode = (hashCode*397) ^ (AreaCode != null ? AreaCode.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((GeoIPCityBo) obj);
        }

        public static bool operator ==(GeoIPCityBo obj1, object obj2)
        {
            return Equals(obj1, obj2);
        }

        public static bool operator !=(GeoIPCityBo obj1, object obj2)
        {
            return !(obj1 == obj2);
        }
    }
}