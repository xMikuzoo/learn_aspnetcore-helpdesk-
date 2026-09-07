using FluentValidation;

namespace Helpdesk.Features.Tickets.Commands.CreateTicket;

public class CreateTicketValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty().WithMessage("Tytuł zgłoszenia nie może być pusty.")
            .MaximumLength(100).WithMessage("Tytuł zgłoszenia może mieć najwyżej 100 znaków.");

        RuleFor(command => command.Priority)
            .NotEmpty().WithMessage("Priorytet jest wymagany.")
            .Must(TicketPriority.IsAllowed).WithMessage($"Priorytet musi być jedną z wartości: {string.Join(", ", TicketPriority.All)}.");
    }
}
