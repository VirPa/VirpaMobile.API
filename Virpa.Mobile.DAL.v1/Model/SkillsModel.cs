using System;

namespace Virpa.Mobile.DAL.v1.Model {

    public class GetSkillsModel {
        
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}