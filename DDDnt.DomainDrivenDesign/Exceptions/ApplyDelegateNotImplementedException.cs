namespace DDDnt.DomainDrivenDesign.Exceptions;

public class ApplyDelegateNotImplementedException : ArgumentNullException
{
    public ApplyDelegateNotImplementedException(string? paramName) : base(paramName) { }

    public ApplyDelegateNotImplementedException(string? paramName, string? message) : base(paramName, message) { }
}
