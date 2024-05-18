using System.Security.Claims;

namespace DDDnt.DomainDrivenDesign.ValueObjects;

public record User(string Name, string? Email, IEnumerable<Claim> Claims) : ValueObject
{
    public User(string name) : this(name, null, [])
    {

    }
}
