using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Principal;

namespace Virpa.Mobile.API.v1.Controllers {

    public class BaseController : Microsoft.AspNetCore.Mvc.Controller {
        private string _identityUser;

        protected string Uri;

        protected string UserAgent;

        protected string Version;

        private IIdentity _userIdentity;

        private const string Urlhelper = "UrlHelper";

        public override void OnActionExecuting(ActionExecutingContext context) {

            _identityUser = HttpContext.User.Identity.Name;

            Uri = HttpContext.Request.Host.Value;

            UserAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            Version = HttpContext.Request.Headers["Version"].ToString();

            _userIdentity = HttpContext.User.Identity;

            context.HttpContext.Items[Urlhelper] = Url;

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
