namespace ManagedCode.IdGenerator.NewId;

public interface INewIdFormatter
{
    string Format(in byte[] bytes);
}