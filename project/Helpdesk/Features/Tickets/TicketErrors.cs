using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets;

public static class TicketErrors
{
    public static Error NotFound(int id) =>
        new("ticket.not_found", $"Zgłoszenie {id} nie istnieje.", ErrorType.NotFound);

    public static readonly Error EmptyTitle =
        new("ticket.empty_title", "Tytuł zgłoszenia nie może być pusty.", ErrorType.Validation);
}
