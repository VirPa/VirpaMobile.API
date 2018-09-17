using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.API.v1.Controllers {

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Followed")]
    [ApiVersion("1.0")]
    public class FollowedController : BaseController {

        #region Initialization

        private readonly List<string> _infos = new List<string>();

        private readonly IMyFollowers _myFollowers;

        #endregion

        #region Constructor

        public FollowedController(IMyFollowers myFollowers) {

            _myFollowers = myFollowers;
        }

        #endregion

        #region Get

        [HttpGet]
        public async Task<IActionResult> GetMyFollowed() {

            var model = new GetMyFollowersModel {
                Email = UserEmail
            };

            var fetchedFollowers = await _myFollowers.GetFollowed(model);

            return Ok(fetchedFollowers);
        }

        #endregion
    }
}
