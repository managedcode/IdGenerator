namespace ManagedCode.IdGenerator.Hashids;

public class NoResultException : Exception
{
    public NoResultException()
    {
    }

    public NoResultException(string message) : base(message)
    {
    }
}