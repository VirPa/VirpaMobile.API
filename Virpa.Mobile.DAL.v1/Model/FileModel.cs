using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Virpa.Mobile.DAL.v1.Model {

    public class PostFilesModel {

        public string FileId { get; set; }

        public FileDetails File { get; set; }
    }

    public class GetFiles {

        public string Email { get; set; }

        public int Type { get; set; }
    }

    public class GetFilesResponse {

        public List<GetFilesListResponse> Files { get; set; }
    }

    public class GetFilesListResponse {
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
        public DateTime? CreatedAt { get; set; }
    }

    public class FileBase64Model {

        public List<FileDetails> Files { get; set; }

        [JsonIgnore]
        public string FeedId { get; set; }

        [JsonIgnore]
        public string FileId { get; set; }

        [JsonIgnore]
        public string Email { get; set; }

        [JsonIgnore]
        public int Type { get; set; }
    }

    public class FileDetails {

        public string Name { get; set; }

        public string Base64 { get; set; }
    }

    public class DeleteFiles {

        [JsonIgnore]
        public string Email { get; set; }

        public List<FileToBeDeleted> Files { get; set; }
    }

    public class FileToBeDeleted {

        public string FileId { get; set; }
    }
}