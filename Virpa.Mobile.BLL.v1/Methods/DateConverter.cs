using Microsoft.Extensions.Options;
using System;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Methods {
    public class DateConverter {

        private readonly IOptions<Manifest> _options;

        public DateConverter(IOptions<Manifest> options) {
            _options = options;
        }

        #region Initial Version

        public DateTime ToUtcPlus8(DateTime dateTime) {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime,
                TimeZoneInfo.FindSystemTimeZoneById("UTC+08"));
        }

        #endregion
    }
}