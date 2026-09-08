using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets;

public static class TicketErrors
{
    public static Error NotFound(int id) =>
        new("ticket.not_found", $"Zgłoszenie {id} nie istnieje.", ErrorType.NotFound);

    public static Error UnknownRequester(string login) =>
        new("ticket.unknown_requester", $"Zgłaszający „{login}” nie istnieje.", ErrorType.NotFound);

    public static Error DuplicateUnresolvedTitle(string title) =>
        new("ticket.duplicate_unresolved_title", $"Masz już nierozwiązane zgłoszenie o tytule „{title}”.", ErrorType.Conflict);
}
