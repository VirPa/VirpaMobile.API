using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {
    public interface IMyFeeds {

        Task<CustomResponse<GetMyFeedsResponseModel>> GetMyFeeds(GetMyFeedsModel model);

        Task<CustomResponse<PostMyFeedResponseModel>> PostMyFeed(PostMyFeedModel model);
    }
}