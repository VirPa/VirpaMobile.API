using System;
using System.Collections.Generic;

namespace Virpa.Mobile.DAL.v1.Entities.Mobile
{
    public partial class FeedBidders
    {
        public string Id { get; set; }
        public string FeedId { get; set; }
        public string UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? Status { get; set; }
    }
}
