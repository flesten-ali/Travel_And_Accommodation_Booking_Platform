using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using TABP.Domain.Interfaces.Services.Email;

namespace TABP.Infrastructure.Services.Email;
public class EmailSenderService : IEmailSenderService
{
    private readonly SMTPConfig _smtpConfig;

    public EmailSenderService(SMTPConfig smtpConfig)
    {
        _smtpConfig = smtpConfig;
    }

    public async Task SendEmailAsync(string recipient, string subject, string body, List<EmailAttachment> emailAttachments)
    {
        var emailToSend = new MimeMessage();
        emailToSend.From.Add(MailboxAddress.Parse(_smtpConfig.From));
        emailToSend.To.Add(MailboxAddress.Parse(recipient));
        emailToSend.Subject = subject;
        BodyBuilder bodyBuilder = new() { HtmlBody = body };

        if (emailAttachments != null)
        {
            foreach (var emailAttachment in emailAttachments)
            {
                bodyBuilder.Attachments.Add(emailAttachment.FileName,
                                        emailAttachment.FileContent,
                                        ContentType.Parse(emailAttachment.ContentType));
            }
        }

        emailToSend.Body = bodyBuilder.ToMessageBody();

        var smtp = new SmtpClient();

        await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_smtpConfig.From, _smtpConfig.Password);
        await smtp.SendAsync(emailToSend);
        await smtp.DisconnectAsync(true);
    }
}
