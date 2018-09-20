using Newtonsoft.Json;

namespace Virpa.Mobile.DAL.v1.Model {

    public class PinLocationModel {

        [JsonIgnore]
        public string Email { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Address { get; set; }

        public string CityName { get; set; }

        public string State { get; set; }

        public string CountryName { get; set; }

        public string PostalCode { get; set; }

        public string IpAddress { get; set; }

        public string MacAddress { get; set; }
    }

    public class PinLocationResponseModel {

        public PinLocationDetailResponseModel Location { get; set; }
    }

    public class PinLocationDetailResponseModel {

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string Address { get; set; }

        public string CityName { get; set; }

        public string State { get; set; }

        public string CountryName { get; set; }

        public string PostalCode { get; set; }

        public string IpAddress { get; set; }

        public string MacAddress { get; set; }
    }
}