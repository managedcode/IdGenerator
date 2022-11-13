namespace ManagedCode.IdGenerator.NewId;

public interface IWorkerIdProvider
{
    byte[] GetWorkerId(int index);
}