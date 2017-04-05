namespace ScreeningAutomation.API.Services
{
    using System.Threading.Tasks;
    using MimeKit;
    using MimeKit.Text;
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Options;
    using Options;

    public class EmailSender : IEmailSender
    {
        private readonly IOptions<EmailSenderOptions> _emailOptionsAccessor;

        public EmailSender(IOptions<EmailSenderOptions> emailOptioinsAccessor)
        {
            _emailOptionsAccessor = emailOptioinsAccessor;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Screening automation tool", "DoNotReply@screening.microsoft.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Text) { Text = message };
            using (var client = new SmtpClient())
            {
                client.LocalDomain = "some.domain.com";
                await
                    client.ConnectAsync(_emailOptionsAccessor.Value.Server, _emailOptionsAccessor.Value.Port, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                await
                    client.AuthenticateAsync(_emailOptionsAccessor.Value.Credentials.Address,
                        _emailOptionsAccessor.Value.Credentials.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
