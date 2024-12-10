using domain.Menus.Aggregates;
using domain.Menus.ValueObjects.Identifiers;
using sharedTests.DataFakers;

namespace integrationTests.EventsStorage
{
    [TestFixture]
    internal sealed class MenuChangedEventsTests : BaseEventInfoStorageTests<Menu, MenuId>
    {
        protected override Menu CreateAggregate()
            => MenuDataFaker.ValidMenu();
    }
}
