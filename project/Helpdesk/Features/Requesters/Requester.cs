namespace Helpdesk.Features.Requesters;

public class Requester
{
    private Requester(string login, string displayName)
    {
        Login = login;
        DisplayName = displayName;
    }

    public int Id { get; private set; }
    public string Login { get; private set; }
    public string DisplayName { get; private set; }

    public static Requester Create(string login, string displayName) => new(login, displayName);
}
