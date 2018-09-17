using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.DataManagers.Interface {
    public interface IFeedsDataManager {

        GetMyFeedsResponseModel GetMyFeedsCreated(GetMyFeedsModel model);

        GetMyFeedsResponseModel GetMyFeedsFromFollowed(GetMyFeedsModel model);

        GetMyFeedsResponseModel GetMyWallFeeds(GetMyFeedsModel model);
    }
}
