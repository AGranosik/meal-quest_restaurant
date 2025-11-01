using core.SimpleTypes;
using domain.Common.DomainImplementationTypes;
using domain.Common.ValueTypes.Strings;
using domain.Menus.ValueObjects.Identifiers;

namespace domain.Menus.Aggregates.Entities;

public class Category : Entity<CategoryId>
{
    public NotEmptyString Name { get; set; }
    public Category(NotEmptyString name)
    {
        Name = name ?? throw new ArgumentException(nameof(name));
    }
    
    protected Category() : base(){}
}