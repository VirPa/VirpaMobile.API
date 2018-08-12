using System;
using System.Collections.Generic;

namespace Virpa.Mobile.DAL.v1.Entities.Mobile
{
    public partial class FeedMessages
    {
        public string Id { get; set; }
        public string FeedId { get; set; }
        public string FeederId { get; set; }
        public string BidderId { get; set; }
        public string Message { get; set; }
        public string Turn { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
