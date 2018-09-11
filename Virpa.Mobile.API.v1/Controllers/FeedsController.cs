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
        private readonly IMyFiles _myFiles;

        private readonly ResponseBadRequest _badRequest;
        private readonly FeedsModelValidator _feedsModelValidator;

        #endregion

        #region Constructor

        public FeedsController(IMyFeeds myFeeds,
            IMyFiles myFiles,
            ResponseBadRequest badRequest,
            FeedsModelValidator feedsModelValidator) {

            _myFeeds = myFeeds;
            _myFiles = myFiles;

            _badRequest = badRequest;
            _feedsModelValidator = feedsModelValidator;
        }

        #endregion

        #region Get

        [HttpGet]
        public async Task<IActionResult> GetFeeds() {

            var model = new GetMyFeedsModel {
                Email = UserEmail
            };

            var fetchedMyFeeds = await _myFeeds.GetFeeds(model);

            return Ok(fetchedMyFeeds);
        }

        [HttpGet("Wall", Name = "Wall")]
        public async Task<IActionResult> GetMyWallFeeds() {

            var model = new GetMyFeedsModel {
                Email = UserEmail
            };

            var fetchedMyFeeds = await _myFeeds.GetMyWallFeeds(model);

            return Ok(fetchedMyFeeds);
        }

        [HttpGet("Wall/{userid}", Name = "WallByUser")]
        public async Task<IActionResult> GetWallFeeds(string userid) {

            var model = new GetMyFeedsModel {
                UserId = userid,
                ByUser = true
            };

            var fetchedMyFeeds = await _myFeeds.GetMyWallFeeds(model);

            return Ok(fetchedMyFeeds);
        }

        #endregion

        #region Post

        [HttpPost]
        public async Task<IActionResult> PostMyFeed([FromBody] PostMyFeedModel model) {

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

        [HttpPost("CoverPhoto/Change", Name = "ChangeFeedCoverPhoto")]
        public async Task<IActionResult> UpdateMyFeedCoverPhoto([FromBody] PostFilesModel postModel) {

            var model = new FileBase64Model {
                Files = new List<FileDetails> {
                    new FileDetails {
                        Name = postModel.File.Name,
                        Base64 = postModel.File.Base64
                    }
                },
                Email = UserEmail,
                FileId = postModel.FileId,
                Type = 1
            };

            #region Validate Model

            if (model.Files == null) {
                _infos.Add(_badRequest.ShowError(ResponseBadRequest.ErrFileEmpty).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            var changedCoverPhoto = await _myFiles.SaveFiles(model);

            return Ok(changedCoverPhoto);
        }

        #endregion
    }
}
