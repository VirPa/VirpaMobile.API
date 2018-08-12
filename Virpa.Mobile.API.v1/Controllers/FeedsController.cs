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
    [Route("Feeds")]
    [ApiVersion("1.0")]
    public class FeedsController : BaseController {

        #region Initialization

        private readonly List<string> _infos = new List<string>();

        private readonly IMyFeeds _myFeeds;

        private readonly ResponseBadRequest _badRequest;
        private readonly FeedsModelValidator _feedsModelValidator;
        private readonly FeedCoverPhotoModelValidator _feedCoverPhotoModelValidator;

        #endregion

        #region Constructor

        public FeedsController(IMyFeeds myFeeds,
            ResponseBadRequest badRequest,
            FeedsModelValidator feedsModelValidator,
            FeedCoverPhotoModelValidator feedCoverPhotoModelValidator) {

            _myFeeds = myFeeds;
            _badRequest = badRequest;
            _feedsModelValidator = feedsModelValidator;
            _feedCoverPhotoModelValidator = feedCoverPhotoModelValidator;
        }

        #endregion

        #region Get

        [HttpGet("MyFeeds", Name = "MyFeeds")]
        public async Task<IActionResult> GetMySkills() {

            var model = new GetMyFeedsModel() {
                Email = UserEmail
            };

            var fetchedMyFeeds = await _myFeeds.GetMyFeeds(model);

            return Ok(fetchedMyFeeds);
        }

        #endregion

        #region Post

        [HttpPost("MyFeed", Name = "MyFeed")]
        public async Task<IActionResult> PostMyFeed(PostMyFeedModel model) {

            #region Validate Model

            var userInputValidated = _feedsModelValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage), 30).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            model.Email = UserEmail;

            var postedMyFeed = await _myFeeds.PostMyFeed(model);

            return Ok(postedMyFeed);
        }

        [HttpPost("MyFeed/CoverPhoto/Update", Name = "UpdateMyFeedCoverPhoto")]
        public async Task<IActionResult> UpdateMyFeedCoverPhoto(UpdateCoverPhotoModel model) {

            #region Validate Model

            var userInputValidated = _feedCoverPhotoModelValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage), 30).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            var updatedCoverPhoto = await _myFeeds.UpdateMyFeedCoverPhoto(model);

            return Ok(updatedCoverPhoto);
        }

        #endregion
    }
}
