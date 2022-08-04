using Khandon.Core.Interfaces.User.Identity;
using Khandon.Shared.Dto.Base.User;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Infrastructure.Users.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig emailConfig;

        public EmailService(IOptionsMonitor<EmailConfig> emailConfig)
        {
            this.emailConfig = emailConfig.CurrentValue;
        }

        public bool SendMail(string Name, string Title, string Email, string Body)
        {
            // Instantiate mimemessag
            var message = new MimeMessage();

            // From Address -- از کدوم ایمیل؟
            message.From.Add(new MailboxAddress("Khandon|خآندون", emailConfig.Email));

            // To Address -- به کدوم ایمیل؟
            message.To.Add(new MailboxAddress(Name, Email));

            // Subject  --- موضوع
            message.Subject = Title;

            // Body -- متن اصلی پیام

            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = Body
            };


            // Configure and send email
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(emailConfig.SMTP_Address, emailConfig.SMTP_Port, emailConfig.SSL);
                    client.Authenticate(emailConfig.Email, emailConfig.Password);

                    client.Send(message);
                    client.Disconnect(true);

                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
    }
}
