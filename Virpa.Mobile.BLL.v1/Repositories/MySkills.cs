using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.DAL.v1.Entities.Mobile;
using Virpa.Mobile.DAL.v1.Identity;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories {
    internal class MySkills : IMySkills {

        #region Initialization

        private readonly List<string> _infos = new List<string>();
        
        private readonly IMapper _mapper;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VirpaMobileContext _context;

        #endregion

        #region Constructor

        public MySkills(IMapper mapper,
            UserManager<ApplicationUser> userManager,
            VirpaMobileContext context) {
            
            _mapper = mapper;

            _userManager = userManager;
            _context = context;
        }

        #endregion
        
        #region Get

        public CustomResponse<List<GetSkillsModel>> GetSkills() {

            var skills = _context.Skills.Where(s => s.IsActive == true).ToList();

            return new CustomResponse<List<GetSkillsModel>> {
                Succeed = true,
                Data = _mapper.Map<List<GetSkillsModel>>(skills)
            };
        }

        public async Task<CustomResponse<List<GetMySkillsResponseModel>>> GetMySkills(GetMySkillsModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var skills = (from us in _context.UserSkills
                         join s in _context.Skills on us.SkillId equals s.Id
                         where us.UserId == user.Id
                         select new GetMySkillsResponseModel {
                             Id = us.SkillId ?? 0,
                             Name = s.Name,
                             Description = s.Description
                         }).ToList();

            return new CustomResponse<List<GetMySkillsResponseModel>> {
                Succeed = true,
                Data = skills
            };
        }
        #endregion

        #region Post

        public async Task<CustomResponse<List<GetMySkillsResponseModel>>> PostMySkills(PostMySkillsModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);

            DeleteRecords();

            var mySkills = new List<UserSkills>();

            foreach (var skill in model.Skills) {

                mySkills.Add(new UserSkills {
                    UserId = user.Id,
                    SkillId = skill.Id,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.UserSkills.AddRangeAsync(mySkills);

            await _context.SaveChangesAsync();

            var fetchedMyRefreshedSkills = GetMySkills(new GetMySkillsModel{Email = model.Email});

            return new CustomResponse<List<GetMySkillsResponseModel>> {
                Succeed = true,
                Data = fetchedMyRefreshedSkills.Result.Data
            };

            #region Local Methods

            void DeleteRecords() {
                var userSkills = _context.UserSkills.Where(us => us.UserId == user.Id).ToList();

                _context.UserSkills.RemoveRange(userSkills);

                _context.SaveChanges();
            }

            #endregion
        }
        #endregion
    }
}