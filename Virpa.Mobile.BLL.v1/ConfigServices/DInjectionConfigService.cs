using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Virpa.Mobile.BLL.v1.Helpers;
using Virpa.Mobile.BLL.v1.Methods;
using Virpa.Mobile.BLL.v1.OtherServices;
using Virpa.Mobile.BLL.v1.OtherServices.Interface;
using Virpa.Mobile.BLL.v1.Repositories;
using Virpa.Mobile.BLL.v1.Repositories.Interface;
using Virpa.Mobile.BLL.v1.Validation;
using Virpa.Mobile.DAL.v1.Entities.Mobile;

namespace Virpa.Mobile.BLL.v1.ConfigServices {

    public static class DInjectionConfigService {
        public static IServiceCollection RegisterDInjection(this IServiceCollection services) {

            services.AddTransient<EmailDaemon>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IMyUser, MyUser>();

            services.AddTransient<IMyAuthentication, MyAuthentication>();

            services.AddTransient<VirpaMobileContext>();

            services.AddTransient<ResponseBadRequest>();

            //OTHER SERVICES
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<IProcessRefreshToken, ProcessRefreshToken>();

            //VALIDATION
            services.AddTransient<UserModelValidator>();

            return services;
        }
    }

}