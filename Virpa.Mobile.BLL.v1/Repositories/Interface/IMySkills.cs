using System.Collections.Generic;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {
    public interface IMySkills {

        CustomResponse<List<GetSkillsModel>> GetSkills();
    }
}