using MailKit.Net.Smtp;
using MimeKit;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public static class EmailSender
    {
        public static async Task SendEmailAsync(List<string> emails)
        {
            var htmlTemplate = "<html>" +
                "<head>" +
                "<style>" +
                "body { font-family: Arial, sans-serif; }" +
                "p { color: red; }" +
                "</style>" +
                "</head>" +
                "<body>" +
                "{TextBlockMain}" +
                "</body>" +
                "</html>";

            var textblockmain1 = "<h1> This is main text block 1 </h1>";
            var textblockmain2 = "<h1> This is main text block 2 </h1>";
            var textblockmain3 = "<h1> This is main text block 3 </h1>";

            foreach (var email in emails)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Sumit", "sumit.gore@sbits.co"));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "Subject of the email";

                var bodyBuilder = new BodyBuilder();
                var textBlockMain = IsSpecialRecipient(email) ? textblockmain1 + textblockmain2 + textblockmain3 : textblockmain1 + textblockmain2;
                bodyBuilder.HtmlBody = htmlTemplate.Replace("{TextBlockMain}", textBlockMain);
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("sumit.gore@sbits.co", "priPAw}Ed94");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
        }

        private static bool IsSpecialRecipient(string email)
        {
            // Implement your conditions for special recipients
            // For example, check if the email address belongs to a certain person or position
            if (email == "sumeet.goliwar@sbits.co" || email.EndsWith("@specialposition.com"))
            {
                return true;
            }

            return false;
        }
    }
}
