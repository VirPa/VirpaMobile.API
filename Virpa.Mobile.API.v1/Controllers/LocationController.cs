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
    [Route("Location")]
    [ApiVersion("1.0")]
    public class LocationController : BaseController {

        #region Initialization

        private readonly List<string> _infos = new List<string>();

        private readonly IMyLocation _myLocation;

        private readonly ResponseBadRequest _badRequest;
        private readonly LocationModelValidator _locationModelValidator;

        #endregion

        #region Constructor

        public LocationController(IMyLocation myLocation,
            ResponseBadRequest badRequest,
            LocationModelValidator locationModelValidator) {

            _myLocation = myLocation;
            _badRequest = badRequest;
            _locationModelValidator = locationModelValidator;
        }

        #endregion

        #region Get

        #endregion

        #region Post

        [HttpPost]
        public async Task<IActionResult> PinLocation([FromBody] PinLocationModel model) {

            #region Validate Model

            var userInputValidated = _locationModelValidator.Validate(model);

            if (!userInputValidated.IsValid) {
                _infos.Add(_badRequest.ShowError(int.Parse(userInputValidated.Errors[0].ErrorMessage), 30).Message);

                return BadRequest(new CustomResponse<string> {
                    Message = _infos
                });
            }

            #endregion

            model.Email = UserEmail;

            var pinnedMyLocation = await _myLocation.PinMyLocation(model);

            return Ok(pinnedMyLocation);
        }
        #endregion
    }
}
