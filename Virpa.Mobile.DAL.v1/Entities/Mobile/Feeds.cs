using System;
using System.Collections.Generic;

namespace Virpa.Mobile.DAL.v1.Entities.Mobile
{
    public partial class Feeds
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int? Type { get; set; }
        public string Body { get; set; }
        public decimal? Budget { get; set; }
        public long? UpVoteCounts { get; set; }
        public long? BiddingCounts { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public int? Status { get; set; }
    }
}
