namespace Helpdesk.Features.Diagnostics;

public interface IStamp { string Id { get; } }
public interface ITransientStamp : IStamp;
public interface IScopedStamp : IStamp;
public interface ISingletonStamp : IStamp;

public class Stamp : ITransientStamp, IScopedStamp, ISingletonStamp
{
    public string Id { get; } = Guid.NewGuid().ToString()[^4..];
}
