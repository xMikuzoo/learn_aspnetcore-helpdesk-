using MediatR;

namespace Helpdesk.Common;

/// <summary>Wspólny marker naszych komend i zapytań; po nim ograniczają się pipeline behaviors.</summary>
public interface ICommandBase;

/// <summary>Operacja zmieniająca stan, bez wyniku.</summary>
public interface ICommand : IRequest, ICommandBase;

/// <summary>Operacja zmieniająca stan, zwracająca TResponse.</summary>
public interface ICommand<out TResponse> : IRequest<TResponse>, ICommandBase;

/// <summary>Zapytanie o dane zwracające TResponse, bez zmiany stanu.</summary>
public interface IQuery<out TResponse> : IRequest<TResponse>, ICommandBase;

/// <summary>Obsługuje jedną operację zmieniającą stan, bez wyniku.</summary>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand;

/// <summary>Obsługuje jedną operację zmieniającą stan, zwracającą TResponse.</summary>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>;

/// <summary>Obsługuje jedno zapytanie o dane, bez zmiany stanu.</summary>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>;
