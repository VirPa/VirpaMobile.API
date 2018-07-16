using System;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.Methods {
    public class EmailDaemon {

        private readonly IOptions<Manifest> _options;

        public EmailDaemon(IOptions<Manifest> options) {
            _options = options;
        }

        public async Task SendEmailAsync(SendEmailModel model) {

            var emailMessage = new MimeMessage {
                From = {
                    new MailboxAddress(_options.Value.Name, _options.Value.SenderEmail)
                },
                To = {
                    new MailboxAddress(_options.Value.Name, model.Recipient)
                },
                Subject = model.Subject,
                Body = new TextPart("html") {
                    Text = model.Body
                }
            };

            using (var client = new SmtpClient()) {

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(_options.Value.Smtp, _options.Value.Port, false).ConfigureAwait(false);

                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_options.Value.SmtpUsername, _options.Value.SmtpPassword);

                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }

        }
        public async Task SendEmailWithTemplateAsync(SendEmailWithTemplateModel model) {

            var randomGuid = Guid.NewGuid().ToString();
            //var cleanUri = _options.Value.Protocol + model.Uri.Replace("api/", "");
            var htmlFilePath = "./html-files/" + model.Template + ".html";

            var builder = new BodyBuilder {
                HtmlBody = File.ReadAllText(htmlFilePath).Replace("^Fullname^", model.FullName)
                    .Replace("^Link^", model.Link)
                    .Replace("^RandomGuid^", randomGuid)
            };

            var emailMessage = new MimeMessage {
                From = {
                    new MailboxAddress(_options.Value.SenderEmail)
                },
                To = {
                    new MailboxAddress(_options.Value.Name, model.Recipient)
                },
                Subject = model.Subject,
                Body = builder.ToMessageBody()
            };

            using (var client = new SmtpClient()) {

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(_options.Value.Smtp, _options.Value.Port, false).ConfigureAwait(false);

                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_options.Value.SmtpUsername, _options.Value.SmtpPassword);

                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);

            }

        }
    }
}