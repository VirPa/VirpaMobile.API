using System;
using System.Collections.Generic;

namespace Virpa.Mobile.DAL.v1.Entities.Mobile
{
    public partial class UserSkills
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public long? SkillId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
