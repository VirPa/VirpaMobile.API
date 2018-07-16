using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Virpa.Mobile.BLL.v1.OtherServices.Interface;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.API.v1.Controllers {

    [AllowAnonymous]
    [Route("Auth")]
    [ApiVersion("1.0")]
    public class AuthenticationController : BaseController {

        private readonly IMyAuthentication _myAuthentication;
        private readonly IProcessRefreshToken _processRefreshToken;

        public AuthenticationController(IMyAuthentication myAuthentication,
            IProcessRefreshToken processRefreshToken) {

            _myAuthentication = myAuthentication;
            _processRefreshToken = processRefreshToken;
        }

        [HttpGet]
        public async Task<IActionResult> Check() {

            return Ok(new {
                Message = "Connected to VirPa API."
            });
        }

        [HttpPost("Sign-In", Name = "SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel model) {

            #region Supply User Agent values

            model.ApiVersion = "1.0";
            model.UserAgent = UserAgent;
            model.AppVersion = Version;

            #endregion

            var signedIn = await _myAuthentication.SignInTheReturnUser(model);

            return Ok(signedIn);
        }

        [HttpPost("Sign-Out", Name = "SignOut")]
        public async Task<IActionResult> SignOut([FromBody] SignOutModel model) {

            #region Supply User Agent values
            model.UserAgent = UserAgent;

            #endregion

            var signedOut = await _myAuthentication.SignOut(model);

            return Ok(signedOut);
        }

        [HttpPost("Generate-Token", Name = "GenerateToken")]
        public async Task<IActionResult> GenerateToken([FromBody] GenerateTokenModel model) {

            #region Supply User Agent values
            model.UserAgent = UserAgent;
            model.AppVersion = Version;
            #endregion

            var generatedToken = await _myAuthentication.GenerateToken(model);

            return Ok(generatedToken);
        }
    }
}
