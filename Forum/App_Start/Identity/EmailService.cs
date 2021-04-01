using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Forum.App_Start.Identity
{
    public class EmailService : IIdentityMessageService
    {
        private readonly string EMAIL_ORIGEM = ConfigurationManager.AppSettings["emailServico:email_remetente"];
        private readonly string SENHA_ORIGEM = ConfigurationManager.AppSettings["emailServico:email_senha"];

        public async Task SendAsync(IdentityMessage message)
        {
            using (var mensagemEmail = new MailMessage())
            {
                mensagemEmail.From = new MailAddress(EMAIL_ORIGEM);

                mensagemEmail.Subject = message.Subject;
                mensagemEmail.To.Add(message.Destination);
                mensagemEmail.Body = message.Body;

                // SMTP - Simple Mail Transport Protocol
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(EMAIL_ORIGEM, SENHA_ORIGEM);

                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;

                    smtpClient.Timeout = 20_000;

                    await smtpClient.SendMailAsync(mensagemEmail);
                }
            }
        }
    }
}