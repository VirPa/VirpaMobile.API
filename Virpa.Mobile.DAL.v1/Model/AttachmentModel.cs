using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Virpa.Mobile.DAL.v1.Model {

    public class AttachmentModel {

        public ICollection<IFormFile> Attachments { get; set; }

        public string Email { get; set; }
    }

    public class SaveAttachments {
        public string Name { get; set; }
        public string CodeName { get; set; }
        public string Extension { get; set; }
        public string FilePath { get; set; }
    }
}