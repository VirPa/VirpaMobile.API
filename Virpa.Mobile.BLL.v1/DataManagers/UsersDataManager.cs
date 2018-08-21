using Dapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;
using System.Linq;
using Virpa.Mobile.BLL.v1.DataManagers.Interface;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.DataManagers {
    public class UsersDataManager : BaseDataManager , IUsersDataManager {

        private readonly IOptions<Manifest> _options;

        private const string SpGetUsers = "sp_GetUsers";

        public UsersDataManager(IOptions<Manifest> options) {
            _options = options;
        }

        public GetUsersModel GetUsers(GetUserModel model) {

            using (var conn = GetSysDbConnection(_options.Value.DefaultConnection)) {

                var getUsersJson = conn.Query<GetUsersDataManagerModel>(SpGetUsers, new {
                    model.UserId
                },
                commandType: CommandType.StoredProcedure).ToList();

                var mappedUsers = JsonConvert.DeserializeObject<GetUsersModel>(getUsersJson.FirstOrDefault()?.Users);

                return mappedUsers;
            }
        }
    }
}
