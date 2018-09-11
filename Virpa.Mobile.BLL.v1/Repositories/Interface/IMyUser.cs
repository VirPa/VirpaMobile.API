using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {

    public interface IMyUser {

        Task<CustomResponse<UserResponse>> GetUser(GetUserModel model);

        Task<CustomResponse<GetUsersModel>> GetUserList(GetUserModel model);

        GetFilesListResponse GetProfilePicture(string userId);

        Task<CustomResponse<CreateUserResponseModel>> CreateUser(CreateUserModel model);

        Task<CustomResponse<UserResponse>> UpdateUser(UpdateUserModel model);

        Task<CustomResponse<ConfirmEmailModel>> SendEmailConfirmation(SendEmailConfirmation model);

        Task<CustomResponse<string>> ConfirmEmail(ConfirmEmailModel model);

        Task<CustomResponse<string>> ChangePassword(ChangePasswordModel model);

        Task<CustomResponse<ForgotPasswordResponseModel>> ForgotPassword(ForgotPasswordModel model);

        Task<CustomResponse<string>> ResetPassword(ResetPasswordModel model);

        Task<CustomResponse<UserResponse>> UpdateBackgroundSummary(UpdateBackgroundSummaryModel model);
    }
}
