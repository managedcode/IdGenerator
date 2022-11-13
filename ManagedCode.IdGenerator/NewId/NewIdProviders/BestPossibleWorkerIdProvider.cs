namespace ManagedCode.IdGenerator.NewId.NewIdProviders;

public class BestPossibleWorkerIdProvider : IWorkerIdProvider
{
    public byte[] GetWorkerId(int index)
    {
        var exceptions = new List<Exception>();

        try
        {
            return new NetworkAddressWorkerIdProvider().GetWorkerId(index);
        }
        catch (Exception ex)
        {
            exceptions.Add(ex);
        }

        try
        {
            return new HostNameHashWorkerIdProvider().GetWorkerId(index);
        }
        catch (Exception ex)
        {
            exceptions.Add(ex);
        }

        throw new AggregateException(exceptions);
    }
}