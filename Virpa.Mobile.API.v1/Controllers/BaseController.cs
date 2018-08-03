using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Security.Principal;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.API.v1.Controllers {

    public class BaseController : Microsoft.AspNetCore.Mvc.Controller {

        private string _identityUser;

        protected string Uri;

        protected string UserAgent;

        protected string Version;

        private IIdentity _userIdentity;

        private const string Urlhelper = "UrlHelper";

        protected string UserEmail;

        public override void OnActionExecuting(ActionExecutingContext context) {

            _identityUser = HttpContext.User.Identity.Name;

            Uri = HttpContext.Request.Host.Value;

            UserAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            Version = HttpContext.Request.Headers["Version"].ToString();

            _userIdentity = HttpContext.User.Identity;

            context.HttpContext.Items[Urlhelper] = Url;

            #region Initializing User

            var accessToken = User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(accessToken)) {
                var jsonPayload = JwtCore.JsonWebToken.Decode(accessToken, "0987654321RUSH_B");

                var jwtUser = JsonConvert.DeserializeObject<JwtModel>(jsonPayload);

                UserEmail = jwtUser.Sub;
            }

            #endregion

            //
            //                       _oo0oo_
            //                      o8888888o
            //                      88" . "88
            //                      (| -_- |)
            //                      0\  =  /0
            //                    ___/`---'\___
            //                  .' \\|     |// '.
            //                 / \\|||  :  |||// \
            //                / _||||| -:- |||||- \
            //               |   | \\\  -  /// |   |
            //               | \_|  ''\---/''  |_/ |
            //               \  .-\__  '-'  ___/-. /
            //             ___'. .'  /--.--\  `. .'___
            //          ."" '<  `.___\_<|>_/___.' >' "".
            //         | | :  `- \`.;`\ _ /`;.`/ - ` : | |
            //         \  \ `_.   \_ __\ /__ _/   .-` /  /
            //     =====`-.____`.___ \_____/___.-`___.-'=====
            //                       `=---='
            //
            //
            //     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //
            //     A Buddha statue to bless your code to be bug free.
            //
        }
    }
}
