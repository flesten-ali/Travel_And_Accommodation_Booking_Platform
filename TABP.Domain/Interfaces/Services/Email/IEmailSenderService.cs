namespace TABP.Domain.Interfaces.Services.Email;
public interface IEmailSenderService
{
    Task SendEmailAsync(string recipient, string subject, string body, List<EmailAttachment> emailAttachments);
}
