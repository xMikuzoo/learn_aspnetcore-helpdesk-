using FluentValidation;
using FluentValidation.Results;

namespace Helpdesk.Common.Validation;

/// <summary>Dekorator ISender: uruchamia walidatory wiadomości przed przekazaniem jej do handlera.</summary>
public class ValidationSender(Sender inner, IServiceProvider provider) : ISender
{
    public async Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        await ValidateAsync(command, cancellationToken);

        return await inner.Send(command, cancellationToken);
    }

    public async Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        await ValidateAsync(query, cancellationToken);

        return await inner.Send(query, cancellationToken);
    }

    private async Task ValidateAsync(object message, CancellationToken cancellationToken)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(message.GetType());
        var validators = provider.GetServices(validatorType).Cast<IValidator>();
        var context = new ValidationContext<object>(message);

        var failures = new List<ValidationFailure>();

        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(context, cancellationToken);
            failures.AddRange(result.Errors);
        }

        if (failures.Count > 0)
        {
            throw new ValidationException(failures);
        }
    }
}
