using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace SimpleEmailApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string SendEmail(string body)
        {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(configuration.GetSection("SenderEmail").Value));
                email.To.Add(MailboxAddress.Parse(configuration.GetSection("ReceiverEmail").Value));
                email.Subject = configuration.GetSection("Subject").Value;
                email.Body = new TextPart(TextFormat.Text) { Text = body };

                using var smtp = new SmtpClient();
                smtp.Connect(configuration.GetSection("Host").Value, Int32.Parse(configuration.GetSection("Port").Value), SecureSocketOptions.Auto);
                smtp.Authenticate(configuration.GetSection("SenderEmail").Value, configuration.GetSection("PassWord").Value);
                smtp.Send(email);
                smtp.Disconnect(true);
                return "Success";
        }
    }
}
