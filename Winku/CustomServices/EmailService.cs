using MimeKit;
using MailKit.Net.Smtp;

namespace Winku.CustomServices
{
    public class EmailService
    {
        public void emailSent(string email,string content)
        {
            var mime = new MimeMessage();

            mime.From.Add(new MailboxAddress("admin", "aqibmushtaqbaba@gmail.com"));
            mime.To.Add(new MailboxAddress("Receiver Name", email));

            mime.Subject ="Password Reset Link";
            mime.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = content
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate("aqibmushtaqbaba@gmail.com", "iepu oisl aeci aaro");

                smtp.Send(mime);
                smtp.Disconnect(true);
            }
        }
    }
}
