using System;
using System.Collections.Generic;

namespace Virpa.Mobile.DAL.v1.Entities.Mobile
{
    public partial class Location
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string State { get; set; }
        public string CountryName { get; set; }
        public string PostalCode { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
