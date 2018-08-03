using AutoMapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.DAL.v1.Entities.Mobile;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories {
    internal class MySkills : IMySkills {

        #region Initialization

        private readonly List<string> _infos = new List<string>();
        
        private readonly IMapper _mapper;
        private readonly VirpaMobileContext _context;

        #endregion

        #region Constructor

        public MySkills(IMapper mapper,
            VirpaMobileContext context) {
            
            _mapper = mapper;
            _context = context;
        }

        #endregion

        public CustomResponse<List<GetSkillsModel>> GetSkills() {

            var skills = _context.Skills.Where(s => s.IsActive == true).ToList();

            return new CustomResponse<List<GetSkillsModel>> {
                Succeed = true,
                Data = _mapper.Map<List<GetSkillsModel>>(skills)
            };
        }
    }
}