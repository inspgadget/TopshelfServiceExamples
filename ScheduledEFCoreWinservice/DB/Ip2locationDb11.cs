using System;
using System.Collections.Generic;

namespace ScheduledEFCoreWinservice.DB
{
    public partial class Ip2locationDb11
    {
        public double IpFrom { get; set; }
        public double IpTo { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ZipCode { get; set; }
        public string TimeZone { get; set; }
    }
}
