using System.Collections.Generic;
using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {
    public interface IMyFiles {

        Task<CustomResponse<GetFilesResponse>> PostFiles(FileModel model);

        Task<CustomResponse<GetFilesResponse>> GetFiles(GetFiles model);

        Task<CustomResponse<GetFilesResponse>> DeleteFiles(DeleteFiles model);
    }
}