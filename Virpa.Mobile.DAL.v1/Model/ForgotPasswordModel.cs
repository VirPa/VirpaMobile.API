using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Virpa.Mobile.DAL.v1.Model {
    public class ForgotPasswordModel {
        public string Email { get; set; }

        [JsonIgnore]
        public string Uri { get; set; }
    }

    public class ForgotPasswordResponseModel {
        public string UserId { get; set; }

        public string Token { get; set; }
    }

    public class ResetPasswordModel {
        public string UserId { get; set; }

        public string Token { get; set; }

        public string NewPassword { get; set; }
    }
}
