using System.Collections.Generic;
using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Repositories.Interface {
    public interface IMyAttachment {

        Task<CustomResponse<GetAttachmentsResponse>> Attach(AttachmentModel model);

        Task<CustomResponse<GetAttachmentsResponse>> GetAttachments(GetAttachments model);

        Task<CustomResponse<GetAttachmentsResponse>> DeleteAttachments(DeleteAttachments model);
    }
}