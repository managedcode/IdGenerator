namespace ManagedCode.IdGenerator.NewId;

public interface INewIdGenerator
{
    NewId Next();

    ArraySegment<NewId> Next(NewId[] ids, int index, int count);
    Guid NextGuid();
    Guid NextSequentialGuid();
}