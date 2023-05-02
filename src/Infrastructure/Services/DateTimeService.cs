using WOF.Application.Common.Interfaces;

namespace WOF.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
