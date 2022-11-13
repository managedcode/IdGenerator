namespace ManagedCode.IdGenerator.NewId;

public interface IProcessIdProvider
{
    byte[] GetProcessId();
}