using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {
    public interface IMyLocation {

        Task<CustomResponse<PinLocationResponseModel>> PinMyLocation(PinLocationModel model);
    }
}