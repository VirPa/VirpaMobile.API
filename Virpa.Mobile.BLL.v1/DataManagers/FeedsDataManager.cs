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

        private const string SpGetMyFeedsCreated = "sp_GetMyFeeds_Created";
        private const string SpGetMyFeedsFromFollowed = "sp_GetMyFeeds_FromFollowed";
        private const string SpGetMyWallFeeds = "sp_GetMyWallFeeds";

        public FeedsDataManager(IOptions<Manifest> options) {
            _options = options;
        }

        public GetMyFeedsResponseModel GetMyFeedsCreated(GetMyFeedsModel model) {

            using (var conn = GetSysDbConnection(_options.Value.DefaultConnection)) {

                var myFeedsJson = conn.Query<GetMyFeedsDataManagerModel>(SpGetMyFeedsCreated, new {
                    model.UserId
                },
                commandType: CommandType.StoredProcedure).ToList();

                var mappedMyFeeds = JsonConvert.DeserializeObject<GetMyFeedsResponseModel>(myFeedsJson.FirstOrDefault()?.MyFeeds);

                return mappedMyFeeds;
            }
        }

        public GetMyFeedsResponseModel GetMyFeedsFromFollowed(GetMyFeedsModel model) {

            using (var conn = GetSysDbConnection(_options.Value.DefaultConnection)) {

                var myFeedsJson = conn.Query<GetMyFeedsDataManagerModel>(SpGetMyFeedsFromFollowed, new {
                        model.UserId
                    },
                    commandType: CommandType.StoredProcedure).ToList();

                var mappedMyFeeds = JsonConvert.DeserializeObject<GetMyFeedsResponseModel>(myFeedsJson.FirstOrDefault()?.MyFeeds);

                return mappedMyFeeds;
            }
        }

        public GetMyFeedsResponseModel GetMyWallFeeds(GetMyFeedsModel model) {

            using (var conn = GetSysDbConnection(_options.Value.DefaultConnection)) {

                var myFeedsJson = conn.Query<GetMyFeedsDataManagerModel>(SpGetMyWallFeeds, new {
                        model.UserId
                    },
                    commandType: CommandType.StoredProcedure).ToList();

                var mappedMyFeeds = JsonConvert.DeserializeObject<GetMyFeedsResponseModel>(myFeedsJson.FirstOrDefault()?.MyFeeds);

                return mappedMyFeeds;
            }
        }
    }
}
