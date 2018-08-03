using System.Collections.Generic;
using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {
    public interface IMyAttachment {

        Task<CustomResponse<List<SaveAttachments>>> Attach(AttachmentModel model);
    }
}