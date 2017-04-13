using CICTED.Domain.Infrastucture.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using MimeKit.Text;

namespace CICTED.Domain.Infrastucture.Services
{
    public class EmailServices : IEmailServices
    {
        public async Task<bool> EnviarEmail(string email, string link)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("CICTED"));
                message.To.Add(new MailboxAddress(email));
                message.Subject = "CICTED";

                var builder = new BodyBuilder();// { TextBody = message };
                builder.HtmlBody = $"Confirme sua conta clicando aqui <a href=\"{link}\">link</a>";

                message.Body = builder.ToMessageBody();
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.AuthenticationMechanisms.Remove("NTLM");
                    client.Connect("smtp.gmail.com", 587);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("camposbruna34@gmail.com", "camposbruna321");

                    client.Send(message);
                    client.Disconnect(true);

                }
                return true;
        }catch(Exception ex)
            {
                return false;
            }
        }

    }
}
