/// <summary>Kto wykonuje bieżące żądanie.</summary>
public interface ICurrentUser
{
    string Name { get; }
}

/// <summary>Uproszczenie na potrzeby kursu: użytkownik z nagłówka X-User, nie z tokenu.</summary>
public class HeaderCurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    public string Name =>
        accessor.HttpContext?.Request.Headers["X-User"].ToString() is { Length: > 0 } name
            ? name
            : "anonim";
}
