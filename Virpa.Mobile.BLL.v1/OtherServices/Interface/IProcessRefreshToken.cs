using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.OtherServices.Interface {
    public interface IProcessRefreshToken {

        Task<CustomResponse<TokenResource>> GenerateRefreshToken(RefreshTokenGetModel model);
    }
}
