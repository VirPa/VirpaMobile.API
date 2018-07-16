using System;
using Newtonsoft.Json;

namespace Virpa.Mobile.DAL.v1.Model {

    public class CreateUserModel {

        public string Fullname { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class UpdateUserModel {

        public string UserId { get; set; }

        public string Fullname { get; set; }

        public string MobileNumber { get; set; }
    }

    public class GetUserModel {

        public string UserId { get; set; }
    }

    public class UserResponse {

        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Fullname { get; set; }

        public string MobileNumber { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}