using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {
    public interface IMyFollowers {

        Task<CustomResponse<GetMyFollowersResponseModel>> GetFollowers(GetMyFollowersModel model);

        Task<CustomResponse<GetMyFollowedResponseModel>> GetFollowed(GetMyFollowersModel model);

        Task<CustomResponse<PostMyFollowerResponseModel>> PostFollower(PostMyFollowerModel model);

        Task<CustomResponse<PostMyFollowerResponseModel>> UnFollow(PostMyFollowerModel model);
    }
}