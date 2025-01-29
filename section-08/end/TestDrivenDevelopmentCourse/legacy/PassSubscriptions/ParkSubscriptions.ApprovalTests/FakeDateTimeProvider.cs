namespace ParkSubscriptions.ApprovalTests;

internal class FakeDateTimeProvider : IDateTimeProvider
{
    public FakeDateTimeProvider(DateTime dateTime)
    {
        Now = dateTime;
    }
    public DateTime Now { get; }
}