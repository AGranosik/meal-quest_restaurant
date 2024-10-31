using domain.Menus.Aggregates;
using domain.Menus.ValueObjects.Identifiers;

namespace infrastructure.EventStorage.DatabaseModels.Configurations
{
    internal class MenuEventConfiguration : DomainEventModelConfiguration<Menu, MenuId>
    {
    }
}
