using Restaurant.Domain.Common.BaseTypes;

namespace Restaurant.Domain.Common.DomainImplementationTypes
{
    public abstract record DomainEvent<T>(T Id)
        where T : ValueObject<T>;
}
