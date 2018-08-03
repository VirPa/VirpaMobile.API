using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Virpa.Mobile.BLL.v1.Repositories.Interface;

namespace Virpa.Mobile.API.v1.Controllers {

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Skills")]
    [ApiVersion("1.0")]
    public class SkillsController : BaseController {

        private readonly List<string> _infos = new List<string>();

        private readonly IMySkills _mySkills;

        public SkillsController(IMySkills mySkills) {

            _mySkills = mySkills;
        }

        [HttpGet]
        public IActionResult Skills() {

            var fetchedSkills = _mySkills.GetSkills();

            return Ok(fetchedSkills);
        }
    }
}
