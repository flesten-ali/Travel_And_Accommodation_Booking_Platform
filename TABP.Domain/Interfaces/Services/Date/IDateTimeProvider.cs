namespace TABP.Domain.Interfaces.Services.Date;
public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}