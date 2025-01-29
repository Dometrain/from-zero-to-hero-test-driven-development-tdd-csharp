namespace ParkSubscriptions.Tests;

public class DummyDateTimeProvider : IDateTimeProvider
{
    private readonly int _year;

    public DummyDateTimeProvider(int year)
    {
        _year = year;
    }

    public DateTime Now => new(_year, 1, 1);
}