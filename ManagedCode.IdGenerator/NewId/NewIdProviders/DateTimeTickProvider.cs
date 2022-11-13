namespace ManagedCode.IdGenerator.NewId.NewIdProviders;

public class DateTimeTickProvider : ITickProvider
{
    public long Ticks => DateTime.UtcNow.Ticks;
}