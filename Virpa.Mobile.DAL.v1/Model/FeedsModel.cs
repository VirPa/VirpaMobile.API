using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Virpa.Mobile.DAL.v1.Model {

    public class GetMyFeedsModel {
        
        public string Email { get; set; }

        public string UserId { get; set; }

        public bool ByUser { get; set; }
    }

    public class GetMyFeedsDataManagerModel {

        public string MyFeeds { get; set; }
    }

    public class GetMyFeedsResponseModel {

        public List<GetMyFeedsListResponseModel> Feeds { get; set; }
    }

    public class GetMyFeedsListResponseModel {

        public string FeedId { get; set; }

        public int? Type { get; set; }

        public string Body { get; set; }

        public decimal? Budget { get; set; }

        public long UpVoteCounts { get; set; }

        public long BiddingCounts { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? ExpiredAt { get; set; } //Days <= 30

        public int Status { get; set; }

        public List<GetFilesListResponse> CoverPhotos { get; set; }
    }

    public class PostMyFeedModel {

        [JsonIgnore]
        public string Email { get; set; }

        public string FeedId { get; set; }

        public int? Type { get; set; }

        public string Body { get; set; }

        public decimal? Budget { get; set; }

        public int ExpiredOn { get; set; } //Days <= 30

        public FileDetails CoverPhoto { get; set; }
    }

    public class PostMyFeedResponseModel {

        public PostMyFeedDetailResponseModel Feed { get; set; }

        public GetFilesListResponse CoverPhoto { get; set; }
    }

    public class PostMyFeedDetailResponseModel {

        public string FeedId { get; set; }

        public int? Type { get; set; }

        public string Body { get; set; }

        public decimal? Budget { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ExpiredAt { get; set; } //Days <= 30
    }

    public class UpdateCoverPhotoModel {

        public string Id { get; set; }

        public ICollection<IFormFile> CoverPhoto { get; set; }
    }

    public class UpdateCoverPhotoResponse {

        public List<GetFilesListResponse> CoverPhoto { get; set; }
    }
}