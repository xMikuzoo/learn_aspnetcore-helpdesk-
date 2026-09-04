namespace Helpdesk.Common;

public class Sender(IServiceProvider provider) : ISender
{
    public Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default) =>
        Dispatch<TResult>(typeof(ICommandHandler<,>), command, cancellationToken);

    public Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default) =>
        Dispatch<TResult>(typeof(IQueryHandler<,>), query, cancellationToken);

    private Task<TResult> Dispatch<TResult>(Type openHandlerType, object message, CancellationToken cancellationToken)
    {
        var handlerType = openHandlerType.MakeGenericType(message.GetType(), typeof(TResult));
        dynamic handler = provider.GetRequiredService(handlerType);

        return handler.HandleAsync((dynamic)message, cancellationToken);
    }
}
