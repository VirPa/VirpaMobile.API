using System;
using Microsoft.AspNetCore.Identity;

namespace Virpa.Mobile.DAL.v1.Identity {

    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser {

        public string Fullname { get; set; }

        public string MobileNumber { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
