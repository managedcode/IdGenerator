namespace ManagedCode.IdGenerator.NewId;

public interface ITickProvider
{
    long Ticks { get; }
}