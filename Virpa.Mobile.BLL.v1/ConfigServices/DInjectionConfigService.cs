using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Virpa.Mobile.BLL.v1.DataManagers;
using Virpa.Mobile.BLL.v1.DataManagers.Interface;
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

            services.AddTransient<IMyFiles, MyFiles>();

            services.AddTransient<IMySkills, MySkills>();

            services.AddTransient<IMyFeeds, MyFeeds>();

            services.AddTransient<IMyFollowers, MyFollowers>();

            services.AddTransient<VirpaMobileContext>();

            services.AddTransient<ResponseBadRequest>();

            //DATA MANAGERS
            services.AddTransient<IFeedsDataManager, FeedsDataManager>();
            services.AddTransient<IUsersDataManager, UsersDataManager>();

            //OTHER SERVICES
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<IProcessRefreshToken, ProcessRefreshToken>();

            //VALIDATION
            services.AddTransient<UserModelValidator>();
            services.AddTransient<UpdateUserModelValidator>();
            services.AddTransient<SendEmailConfirmationValidator>();
            services.AddTransient<ConfirmEmailValidator>();
            services.AddTransient<ChangePasswordValidator>();
            services.AddTransient<ForgotPasswordValidator>();
            services.AddTransient<ResetPasswordValidator>();

            services.AddTransient<SignInModelValidator>();
            services.AddTransient<SignOutModelValidator>();
            services.AddTransient<GenerateTokenModelValidator>();
            
            services.AddTransient<FileBase64ModelValidator>();

            services.AddTransient<FeedsModelValidator>();
            services.AddTransient<FeedCoverPhotoModelValidator>();

            services.AddTransient<FollowersModelValidator>();

            return services;
        }
    }

}