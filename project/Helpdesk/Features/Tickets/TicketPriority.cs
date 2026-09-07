namespace Helpdesk.Features.Tickets;

public static class TicketPriority
{
    public static readonly string[] All = ["low", "normal", "high"];

    public static bool IsAllowed(string? value) => All.Contains(value);
}
