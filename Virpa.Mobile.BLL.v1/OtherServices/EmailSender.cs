using System.Threading.Tasks;
using Virpa.Mobile.BLL.v1.Methods;
using Virpa.Mobile.BLL.v1.OtherServices.Interface;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.OtherServices {
    public class EmailSender : IEmailSender {

        private readonly EmailDaemon _emailDaemon;

        public EmailSender(EmailDaemon emailDaemon) {

            _emailDaemon = emailDaemon;
        }

        public async Task SendEmailAsync(SendEmailModel model) {

            await _emailDaemon.SendEmailAsync(model);
        }

        public async Task SendEmailWithTemplateAsync(SendEmailWithTemplateModel model) {

            await _emailDaemon.SendEmailWithTemplateAsync(model);
        }
    }
}
