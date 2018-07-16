using System.Threading.Tasks;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.OtherServices.Interface {
    public interface IEmailSender {

        Task SendEmailAsync(SendEmailModel model);

        Task SendEmailWithTemplateAsync(SendEmailWithTemplateModel model);
    }
}
