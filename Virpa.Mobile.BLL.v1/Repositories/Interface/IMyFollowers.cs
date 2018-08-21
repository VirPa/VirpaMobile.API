using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {
    public interface IMyFollowers {

        Task<CustomResponse<GetMyFollowersResponseModel>> GetFollowers(GetMyFollowersModel model);

        Task<CustomResponse<GetMyFollowersResponseModel>> PostFollower(PostMyFollowerModel model);

        Task<CustomResponse<GetMyFollowersResponseModel>> UnFollow(PostMyFollowerModel model);
    }
}