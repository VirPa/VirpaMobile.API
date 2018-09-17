using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Virpa.Mobile.DAL.v1.Model {

    public class GetMyFollowersModel {

        public string Email { get; set; }
    }

    public class GetMyFollowersResponseModel {

        public List<GetMyFollowersListModel> Followers { get; set; }
    }

    public class GetMyFollowersListModel {

        public UserResponse User { get; set; }

        public DateTime? FollowedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public class GetMyFollowedResponseModel {

        public List<GetMyFollowedListModel> Followed { get; set; }
    }

    public class GetMyFollowedListModel {

        public UserResponse User { get; set; }

        public DateTime? FollowedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public class PostMyFollowerModel {

        [JsonIgnore]
        public string Email { get; set; }

        public string FollowedId { get; set; }
    }

    public class PostMyFollowerResponseModel {

        public string FollowedId { get; set; }

        public DateTime FollowedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}