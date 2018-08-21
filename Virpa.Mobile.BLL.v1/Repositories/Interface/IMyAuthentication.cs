using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {
    public interface IMyAuthentication {

        Task<CustomResponse<SignInReponseModel>> SignInTheReturnUser(SignInModel model);

        Task<CustomResponse<string>> SignOut(SignOutModel model);

        Task<CustomResponse<GenerateTokenResponseModel>> GenerateToken(GenerateTokenModel model);
    }
}