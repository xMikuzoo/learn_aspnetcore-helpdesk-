namespace Helpdesk.Common.Errors;

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    Forbidden
}

public record Error(string Code, string Message, ErrorType Type);
