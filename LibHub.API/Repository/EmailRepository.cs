using LibHub.API.Repository.Contracts;
using LibHub.API.Entities;
using LibHub.Models.DTOs;
using MailKit.Security;
using MimeKit;
using System.Runtime.Serialization;
using LibHub.API.Data;
using System.Runtime.CompilerServices;

namespace LibHub.API.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _config;

        public EmailRepository(IConfiguration config)
        {
            _config = config;
        }
        public void SendEmail(EmailDTO request)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sender Name", _config.GetSection("EmailUserName").Value));
            message.To.Add(new MailboxAddress("Recipient Name", request.To));
            message.Subject = request.Subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = request.Body };


            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                client.Authenticate(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);

                client.Send(message);
                client.Disconnect(true);

            }
        }
    }
}
