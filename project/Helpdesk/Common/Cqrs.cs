namespace Helpdesk.Common;

/// <summary>Obsługuje jedną operację zmieniającą stan.</summary>
public interface ICommandHandler<in TCommand, TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>Obsługuje jedno zapytanie o dane, bez zmiany stanu.</summary>
public interface IQueryHandler<in TQuery, TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
