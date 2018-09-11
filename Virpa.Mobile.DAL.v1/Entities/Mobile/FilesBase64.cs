using System;
using System.Collections.Generic;

namespace Virpa.Mobile.DAL.v1.Entities.Mobile
{
    public partial class FilesBase64
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FeedId { get; set; }
        public string Name { get; set; }
        public string CodeName { get; set; }
        public string Extension { get; set; }
        public string FileBase64 { get; set; }
        public int? Type { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsCurrentProfilePicture { get; set; }
    }
}
