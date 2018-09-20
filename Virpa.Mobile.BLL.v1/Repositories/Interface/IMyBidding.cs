using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {
    public interface IMyBidding {

        CustomResponse<GetBiddersResponseModel> GetBidders(GetBiddersModel model);

        Task<CustomResponse<PostBidderResponseModel>> PostBidder(PostBidderModel model);
    }
}