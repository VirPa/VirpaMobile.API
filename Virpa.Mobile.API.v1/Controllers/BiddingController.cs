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
    [Route("Bid")]
    [ApiVersion("1.0")]
    public class BiddingController : BaseController {

        #region Initialization

        private readonly List<string> _infos = new List<string>();

        private readonly IMyBidding _myBidding;

        private readonly ResponseBadRequest _badRequest;
        private readonly BiddingModelValidator _biddingModelValidator;

        #endregion

        #region Constructor

        public BiddingController(IMyBidding myBidding,
            ResponseBadRequest badRequest,
            BiddingModelValidator biddingModelValidator) {

            _myBidding = myBidding;
            _badRequest = badRequest;
            _biddingModelValidator = biddingModelValidator;
        }

        #endregion

        #region Get

        [HttpGet("{feedid}", Name = "GetBidders")]
        public IActionResult GetBidders(string feedid) {

            var model = new GetBiddersModel {
                FeedId = feedid
            };

            var fetchedBidders = _myBidding.GetBidders(model);

            return Ok(fetchedBidders);
        }

        #endregion

        #region Post

        [HttpPost]
        public async Task<IActionResult> PostBidding([FromBody] PostBidderModel model) {

            #region Validate Model

            var userInputValidated = _biddingModelValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage), 30).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            model.Email = UserEmail;

            var postedMyFollower = await _myBidding.PostBidder(model);

            return Ok(postedMyFollower);
        }
        #endregion
    }
}
