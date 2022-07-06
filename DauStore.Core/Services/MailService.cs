using System;
using System.Threading.Tasks;
using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IServices;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DauStore.Core.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings mailSettings;



        // mailSetting được Inject qua dịch vụ hệ thống
        // Có inject Logger để xuất log
        public MailService(IOptions<MailSettings> _mailSettings)
        {
            mailSettings = _mailSettings.Value;
        }

        // Gửi email, theo nội dung trong mailContent
        public void SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                smtp.Send(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            smtp.Disconnect(true);
        }
    }
}
