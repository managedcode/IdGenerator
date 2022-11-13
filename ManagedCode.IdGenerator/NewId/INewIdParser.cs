namespace ManagedCode.IdGenerator.NewId;

public interface INewIdParser
{
    NewId Parse(in string text);
}