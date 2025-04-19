using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using TABP.Domain.Interfaces.Services.Email;

namespace TABP.Infrastructure.Services.Email;

public class EmailSenderService(SMTPConfig smtpConfig) : IEmailSenderService
{
    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="recipient">The recipient email address.</param>
    /// <param name="subject">The email subject.</param>
    /// <param name="body">The email body (HTML format supported).</param>
    /// <param name="emailAttachments">A list of attachments to include in the email.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendEmailAsync(
        string recipient,
        string subject,
        string body,
        List<EmailAttachment> emailAttachments,
        CancellationToken cancellationToken = default)
    {
        var emailToSend = new MimeMessage();
        emailToSend.From.Add(MailboxAddress.Parse(smtpConfig.From));
        emailToSend.To.Add(MailboxAddress.Parse(recipient));
        emailToSend.Subject = subject;

        // Create the email body
        BodyBuilder bodyBuilder = new() { HtmlBody = body };

        // Add attachments if provided
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

        using var smtp = new SmtpClient();

        // Connect to SMTP server with TLS
        await smtp.ConnectAsync(smtpConfig.Host, smtpConfig.Port, SecureSocketOptions.StartTls, cancellationToken);

        // Authenticate with SMTP credentials
        await smtp.AuthenticateAsync(smtpConfig.From, smtpConfig.Password, cancellationToken);

        // Send the email
        await smtp.SendAsync(emailToSend, cancellationToken);

        // Disconnect cleanly
        await smtp.DisconnectAsync(true, cancellationToken);
    }
}