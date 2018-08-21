using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.DataManagers.Interface {
    public interface IUsersDataManager {

        GetUsersModel GetUsers(GetUserModel model);
    }
}
