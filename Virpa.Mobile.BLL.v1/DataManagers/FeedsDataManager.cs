using Dapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;
using System.Linq;
using Virpa.Mobile.BLL.v1.DataManagers.Interface;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.DataManagers {
    public class FeedsDataManager : BaseDataManager , IFeedsDataManager {

        private readonly IOptions<Manifest> _options;

        private const string SpGetMyFeeds = "sp_GetMyFeeds";

        public FeedsDataManager(IOptions<Manifest> options) {
            _options = options;
        }

        public GetMyFeedsResponseModel GetMyFeeds(GetMyFeedsModel model) {

            using (var conn = GetSysDbConnection(_options.Value.DefaultConnection)) {

                var myFeedsJson = conn.Query<GetMyFeedsDataManagerModel>(SpGetMyFeeds, new {
                    model.UserId
                },
                commandType: CommandType.StoredProcedure).ToList();

                var mappedMyFeeds = JsonConvert.DeserializeObject<GetMyFeedsResponseModel>(myFeedsJson.FirstOrDefault()?.MyFeeds);

                return mappedMyFeeds;
            }
        }
    }
}
