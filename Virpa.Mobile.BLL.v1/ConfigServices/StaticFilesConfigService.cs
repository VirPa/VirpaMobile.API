using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Virpa.Mobile.BLL.v1.ConfigServices {

    public static class StaticFilesConfigService {

        public static IApplicationBuilder RegisterStaticFiles(this IApplicationBuilder applicationBuilder) {

            applicationBuilder.UseStaticFiles(new StaticFileOptions {

                #region File Path

                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"logo")),
                RequestPath = new PathString("/logo"),

                #endregion

                #region Caching

                OnPrepareResponse = ctx => {
                    /*publicly cacheable for 10 minutes*/
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                }

                #endregion

            });

            return applicationBuilder;
        }
    }


}