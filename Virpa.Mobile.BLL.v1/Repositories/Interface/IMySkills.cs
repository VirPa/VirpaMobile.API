using System.Collections.Generic;
using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {
    public interface IMySkills {

        CustomResponse<GetSkillsModel> GetSkills();

        Task<CustomResponse<GetMySkillsResponseModel>> GetMySkills(GetMySkillsModel model);

        Task<CustomResponse<GetMySkillsResponseModel>> PostMySkills(PostMySkillsModel model);
    }
}