using System;
using System.Collections.Generic;

namespace Virpa.Mobile.DAL.v1.Entities.Mobile
{
    public partial class Followers
    {
        public string Id { get; set; }
        public string FollowedId { get; set; }
        public string FollowerId { get; set; }
        public DateTime? FollowedAt { get; set; }
    }
}
