using Microsoft.Extensions.Configuration;
using NerdStore.Infra.EmailServices.Interfaces;
using System.Net;
using System.Net.Mail;

namespace NerdStore.Infra.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task EnviarEmail(string emailDestino, string assunto, string corpo)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("dondev.lab@gmail.com");//de
            mail.To.Add(emailDestino); // para
            mail.Subject = assunto; // assunto
            mail.Body = corpo; // mensagem

            // em caso de anexos
            //mail.Attachments.Add(new Attachment(@"C:\teste.txt"));

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                var email = _configuration.GetSection("EmailCredentials.Email").Value;
                var senha = _configuration.GetSection("EmailCredentials.Senha").Value;
                smtp.EnableSsl = true; // GMail requer SSL
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio
                smtp.UseDefaultCredentials = false; // vamos utilizar credencias especificas

                // seu usuário e senha para autenticação
                smtp.Credentials = new NetworkCredential(email, senha);

                // envia o e-mail
                await smtp.SendMailAsync(mail);
            }
        }
    }
}
