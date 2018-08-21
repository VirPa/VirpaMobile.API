using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Virpa.Mobile.BLL.v1.Helpers;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.BLL.v1.Validation;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.API.v1.Controllers {

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Followers")]
    [ApiVersion("1.0")]
    public class FollowersController : BaseController {

        #region Initialization

        private readonly List<string> _infos = new List<string>();

        private readonly IMyFollowers _myFollowers;

        private readonly ResponseBadRequest _badRequest;
        private readonly FollowersModelValidator _followersModelValidator;

        #endregion

        #region Constructor

        public FollowersController(IMyFollowers myFollowers,
            ResponseBadRequest badRequest,
            FollowersModelValidator followersModelValidator) {

            _myFollowers = myFollowers;
            _badRequest = badRequest;
            _followersModelValidator = followersModelValidator;
        }

        #endregion

        #region Get

        [HttpGet]
        public async Task<IActionResult> GetMyFollowers() {

            var model = new GetMyFollowersModel {
                Email = UserEmail
            };

            var fetchedFollowers = await _myFollowers.GetFollowers(model);

            return Ok(fetchedFollowers);
        }

        #endregion

        #region Post

        [HttpPost]
        public async Task<IActionResult> PostMyFollower([FromBody] PostMyFollowerModel model) {

            #region Validate Model

            var userInputValidated = _followersModelValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage), 30).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            model.Email = UserEmail;

            var postedMyFollower = await _myFollowers.PostFollower(model);

            return Ok(postedMyFollower);
        }

        [HttpPost("UnFollow", Name = "UnFollow")]
        public async Task<IActionResult> UnFollow([FromBody] PostMyFollowerModel model) {

            #region Validate Model

            var userInputValidated = _followersModelValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage), 30).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            model.Email = UserEmail;

            var unFollowed = await _myFollowers.UnFollow(model);

            return Ok(unFollowed);
        }
        #endregion
    }
}
