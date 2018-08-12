using System;
using System.Collections.Generic;

namespace Virpa.Mobile.DAL.v1.Entities.Mobile
{
    public partial class ReferenceData
    {
        public long Id { get; set; }
        public string RefKey { get; set; }
        public string RefId { get; set; }
        public string Value { get; set; }
    }
}
