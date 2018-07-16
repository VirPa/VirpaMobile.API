using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.Swagger;

namespace Virpa.Mobile.BLL.v1.ConfigServices {

    public static class SwaggerConfigService {
        public static IServiceCollection RegisterSwagger(this IServiceCollection services) {

            return services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1",
                    new Info {
                        Title = "VirPA API Technology",
                        Description = "Endpoints to access VirPA business logic.",
                        Version = "Last updated on July 02, 2018"
                    });
            });
        }

        public static IApplicationBuilder RegisterSwagger(this IApplicationBuilder applicationBuilder) {

            applicationBuilder.UseSwagger();

            applicationBuilder.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "VirPA API v1");
                c.RoutePrefix = "docs/swagger";
            });

            return applicationBuilder;
        }
    }

}