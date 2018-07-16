using Newtonsoft.Json;

namespace Virpa.Mobile.DAL.v1.Model {

    public class SignInModel {
        
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        [JsonIgnore]
        public string UserAgent { get; set; }

        [JsonIgnore]
        public string AppVersion { get; set; }

        [JsonIgnore]
        public string ApiVersion { get; set; }
    }

    public class SignInReponseModel {

        public SignInReturnToken Authorization { get; set; }

        public UserResponse User { get; set; }
    }

    public class SignInReturnToken {

        public TokenResource SessionToken { get; set; }

        public TokenResource RefreshToken { get; set; }
    }

    public class SignOutModel {

        public string Email { get; set; }

        [JsonIgnore]
        public string UserAgent { get; set; }
    }

    public class GenerateTokenModel {

        [JsonProperty(PropertyName = "email")]
        public string UserName { get; set; }

        public GenerateTokenResourceModel TokenResource { get; set; }

        [JsonIgnore]
        public string UserAgent { get; set; }

        [JsonIgnore]
        public string AppVersion { get; set; }
    }

    public class GenerateTokenResourceModel {

        //Info: refresh/session
        public string Type { get; set; }

        public string Token { get; set; }
    }
}