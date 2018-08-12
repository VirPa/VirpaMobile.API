using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Virpa.Mobile.DAL.v1.Model {

    public class AttachmentModel {

        public ICollection<IFormFile> Attachments { get; set; }

        public string Email { get; set; }

        public string FeedId { get; set; }

        [JsonIgnore]
        public int Type { get; set; }
    }

    public class GetAttachments {

        public string Email { get; set; }

        public int Type { get; set; }
    }

    public class GetAttachmentsResponse {

        public List<GetAttachmentsListResponse> Attachments { get; set; }
    }

    public class GetAttachmentsListResponse {
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        public string FeedId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string CodeName { get; set; }
        public string Extension { get; set; }
        public string FilePath { get; set; }
        public int Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class DeleteAttachments {

        [JsonIgnore]
        public string Email { get; set; }

        public List<AttachmentToBeDeleted> Attachments { get; set; }
    }

    public class AttachmentToBeDeleted {

        public string Id { get; set; }
    }
}