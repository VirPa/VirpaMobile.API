using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {
    public interface IMyFeeds {

        Task<CustomResponse<GetMyFeedsResponseModel>> GetFeeds(GetMyFeedsModel model);

        Task<CustomResponse<GetMyFeedsResponseModel>> GetMyWallFeeds(GetMyFeedsModel model);

        Task<CustomResponse<PostMyFeedResponseModel>> PostMyFeed(PostMyFeedModel model);
    }
}