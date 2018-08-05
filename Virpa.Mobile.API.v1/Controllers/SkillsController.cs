using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.API.v1.Controllers {

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Skills")]
    [ApiVersion("1.0")]
    public class SkillsController : BaseController {

        #region Initialization

        private readonly List<string> _infos = new List<string>();

        private readonly IMySkills _mySkills;

        #endregion

        #region Constructor

        public SkillsController(IMySkills mySkills) {

            _mySkills = mySkills;
        }

        #endregion

        #region Get

        [HttpGet]
        public IActionResult GetSkills() {

            var fetchedSkills = _mySkills.GetSkills();

            return Ok(fetchedSkills);
        }

        [HttpGet("MySkills", Name = "MySkills")]
        public async Task<IActionResult> GetMySkills() {

            var model = new GetMySkillsModel {
                Email = UserEmail
            };

            var fetchedMySkills = await _mySkills.GetMySkills(model);

            return Ok(fetchedMySkills);
        }

        #endregion

        #region Post

        [HttpPost("MySkills", Name = "MySkills")]
        public async Task<IActionResult> PostMySkills([FromBody] PostMySkillsModel model) {

            model.Email = UserEmail;

            var postedMySkills = await _mySkills.PostMySkills(model);

            return Ok(postedMySkills);
        }

        #endregion
    }
}
