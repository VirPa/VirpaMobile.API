using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Virpa.Mobile.DAL.v1.Model {

    public class GetSkillsModel {

        public List<GetSkillsListModel> Skills { get; set; }
    }

    public class GetSkillsListModel {

        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public class GetMySkillsModel {

        public string Email { get; set; }
    }

    public class GetMySkillsResponseModel {

        public List<GetMySkillsListResponseModel> Skills { get; set; }
    }

    public class GetMySkillsListResponseModel {

        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class PostMySkillsModel {

        [JsonIgnore]
        public string Email { get; set; }

        public List<SkillsModel> Skills { get; set; }
    }

    public class SkillsModel {

        public long Id { get; set; }

        public string Name { get; set; }

    }
}