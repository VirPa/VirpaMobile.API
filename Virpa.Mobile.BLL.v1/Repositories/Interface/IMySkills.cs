using System.Collections.Generic;
using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {
    public interface IMySkills {

        CustomResponse<List<GetSkillsModel>> GetSkills();

        Task<CustomResponse<List<GetMySkillsResponseModel>>> GetMySkills(GetMySkillsModel model);

        Task<CustomResponse<List<GetMySkillsResponseModel>>> PostMySkills(PostMySkillsModel model);
    }
}