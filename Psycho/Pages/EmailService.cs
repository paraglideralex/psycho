using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System.Net.Mail;

namespace SocialApp.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Trialectica - психологический тест", "trialectica@mail.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;



            var builder = new BodyBuilder();
            builder.TextBody = message;

            // если кто-то захочет прикреплять файл к письму, то делать это здесь. Пока всё просто пишется в письмо, может, так и удобнее
            // builder.Attachments.Add("results.txt"); 

            emailMessage.Body = builder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465, true);
                await client.AuthenticateAsync("trialectica@mail.ru", "TMnMdmWtgg4kmMtvSmHU");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}