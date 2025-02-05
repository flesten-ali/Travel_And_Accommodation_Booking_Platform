namespace TABP.Domain.Interfaces.Services.Email;
public class EmailAttachment
{
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }
    public string ContentType { get; set; }
}
