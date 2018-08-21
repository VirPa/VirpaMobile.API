using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Virpa.Mobile.DAL.v1.Model {

    public class CreateUserModel {

        public string Fullname { get; set; }

        public string MobileNumber { get; set; }

        public string BackgroundSummary { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Device { get; set; }
    }

    public class UpdateUserModel {

        public string UserId { get; set; }

        public string Fullname { get; set; }

        public string MobileNumber { get; set; }

        public string BackgroundSummary { get; set; }
    }

    public class GetUserModel {

        public string UserId { get; set; }
    }

    public class UserResponse {

        public UserDetails User { get; set; }
    }

    public class UserDetails {

        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Fullname { get; set; }

        public string MobileNumber { get; set; }

        public long FollowersCount { get; set; }

        public string BackgroundSummary { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public class ChangeProfilePictureModel {

        public string Id { get; set; }

        public ICollection<IFormFile> CoverPhoto { get; set; }
    }

    public class ChangeProfilePictureResponse {

        public List<GetFilesListResponse> ProfilePicture { get; set; }
    }
}