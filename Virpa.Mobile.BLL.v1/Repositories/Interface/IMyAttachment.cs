using System.Collections.Generic;
using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {
    public interface IMyAttachment {

        Task<CustomResponse<List<GetAttachmentsResponse>>> Attach(AttachmentModel model);

        Task<CustomResponse<List<GetAttachmentsResponse>>> GetAttachments(GetAttachments model);
    }
}