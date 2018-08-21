namespace Virpa.Mobile.DAL.v1.Model {

    public class Manifest {
        public string Name { get; set; }

        public string Protocol { get; set; }

        public string Uri { get; set; }

        public string Api { get; set; }

        public string Docs { get; set; }

        public string Files { get; set; }

        public string ApiKey { get; set; }

        public bool SendEmail { get; set; }

        public bool IsEnabled { get; set; }

        public string DefaultConnection { get; set; }

        public string Key { get; set; }

        public string Issuer { get; set; }

        public int AccessTokenExpiryMins { get; set; }

        public int RefreshTokenExpiryMins { get; set; }

        //Email Notification
        public string Smtp { get; set; }

        public int Port { get; set; }

        public string SmtpUsername { get; set; }

        public string SmtpPassword { get; set; }

        public string SenderEmail { get; set; }

        public string EmailPassword { get; set; }

        //Files
        public string DirectoryPath { get; set; }
    }
}