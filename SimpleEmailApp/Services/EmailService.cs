using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;

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

            using(var email=new MimeMessage())
            {
                // configure the sender, recipient, subject,attachment ,and body of the message

                email.From.Add(MailboxAddress.Parse(configuration.GetSection("SenderEmail").Value));
                email.To.Add(MailboxAddress.Parse(configuration.GetSection("ReceiverEmail").Value));
                email.Subject = configuration.GetSection("Subject").Value;

                var multipart = new Multipart("mixed");

                // add the text body
                multipart.Add(new TextPart(TextFormat.Text)//  set "plain or any format 
                {
                    Text = body
                });

                //Build attachment  Path 

                string FileName = configuration.GetSection("FileName").Value;
                string path = Path.Combine("Files/", FileName);

                // add the attachments
                var attachment = new MimePart()
                {
                    Content = new MimeContent(File.OpenRead(path)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = FileName
                };
                multipart.Add(attachment);

                email.Body = multipart;

                using var smtp = new SmtpClient();
                smtp.Connect(configuration.GetSection("Host").Value, Int32.Parse(configuration.GetSection("Port").Value), SecureSocketOptions.Auto);
                smtp.Authenticate(configuration.GetSection("SenderEmail").Value, configuration.GetSection("PassWord").Value);
                smtp.Send(email);
                smtp.Disconnect(true);
                return "Success";
            }
        }
    }
}
