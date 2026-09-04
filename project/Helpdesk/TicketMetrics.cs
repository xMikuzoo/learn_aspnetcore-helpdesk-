/// <summary>Licznik zgłoszeń utworzonych od startu aplikacji</summary>
public class TicketMetrics
{
    private int _created;

    public int Created => _created;

    public void RecordCreated(ICurrentUser user)
    {
        _created++;
        Console.WriteLine($"[metrics] {user.Name} utworzyl zgloszenie; lacznie od startu: {_created}");
    }
}
