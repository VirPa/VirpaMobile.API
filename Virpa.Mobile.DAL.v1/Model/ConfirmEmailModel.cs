using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Virpa.Mobile.DAL.v1.Model {
    public class SendEmailConfirmation {
        public string Email { get; set; }

        [JsonIgnore]
        public string Uri { get; set; }
    }

    public class ConfirmEmailModel {
        public string UserId { get; set; }

        public string Token { get; set; }
    }
}
