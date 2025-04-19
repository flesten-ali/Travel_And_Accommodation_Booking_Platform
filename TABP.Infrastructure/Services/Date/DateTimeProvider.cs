using TABP.Domain.Interfaces.Services.Date;

namespace TABP.Infrastructure.Services.Date;

public class DateTimeProvider : IDateTimeProvider
{
    private DateTime _date;
    public DateTimeProvider()
    {
        _date = DateTime.UtcNow;
    }
    public DateTime UtcNow => _date;

    public void Set(DateTime dateTime)
    {
        _date = dateTime;
    }
}