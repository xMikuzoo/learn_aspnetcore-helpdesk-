namespace Helpdesk.Common;

/// <summary>Operacja zmieniająca stan, zwracająca TResult.</summary>
public interface ICommand<TResult>;

/// <summary>Zapytanie o dane zwracające TResult, bez zmiany stanu.</summary>
public interface IQuery<TResult>;

/// <summary>Obsługuje jedną operację zmieniającą stan.</summary>
public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>Obsługuje jedno zapytanie o dane, bez zmiany stanu.</summary>
public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
