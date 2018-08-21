﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        private readonly FileModelValidator _fileModelValidator;

        #endregion

        #region Constructor

        public FeedsController(IMyFeeds myFeeds,
            IMyFiles myFiles,
            ResponseBadRequest badRequest,
            FeedsModelValidator feedsModelValidator,
            FileModelValidator fileModelValidator) {

            _myFeeds = myFeeds;
            _myFiles = myFiles;

            _badRequest = badRequest;
            _feedsModelValidator = feedsModelValidator;
            _fileModelValidator = fileModelValidator;
        }

        #endregion

        #region Get

        [HttpGet]
        public async Task<IActionResult> GetMySkills() {

            var model = new GetMyFeedsModel() {
                Email = UserEmail
            };

            var fetchedMyFeeds = await _myFeeds.GetMyFeeds(model);

            return Ok(fetchedMyFeeds);
        }

        #endregion

        #region Post

        [HttpPost]
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

        [HttpPost("CoverPhoto/Change", Name = "ChangeFeedCoverPhoto")]
        public async Task<IActionResult> UpdateMyFeedCoverPhoto(PostFilesModel postModel) {

            var model = new FileModel {
                Files = postModel.Files,
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

            var userInputValidated = _fileModelValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage)).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            var changedCoverPhoto = await _myFiles.PostFiles(model);

            return Ok(changedCoverPhoto);
        }

        #endregion
    }
}
