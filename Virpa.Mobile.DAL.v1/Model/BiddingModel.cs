using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Virpa.Mobile.DAL.v1.Model {

    public class GetBiddersModel {

        public string FeedId { get; set; }
    }

    public class GetBiddersResponseModel {

        public List<GetBiddersListModel> Bidders { get; set; }
    }

    public class GetBiddersListModel {

        public UserResponse User { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? Status { get; set; }
    }

    public class PostBidderModel {

        public string FeedId { get; set; }

        [JsonIgnore]
        public string Email { get; set; }

        public string InitialMessage { get; set; }
    }

    public class PostBidderResponseModel {

        public PostBidderDetailResponseModel Bidder { get; set; }
    }

    public class PostBidderDetailResponseModel {

        public string FeedId { get; set; }

        public string UserId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string InitialMessage { get; set; }

        public int Status { get; set; }
    }
}