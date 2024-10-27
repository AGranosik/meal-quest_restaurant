using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using domain.Common.ValueTypes.Strings;
using domain.Menus.Aggregates.DomainEvents;
using domain.Menus.ValueObjects.Identifiers;
using FluentAssertions;

namespace unitTests.Domain.Menus.Events
{
    [TestFixture]
    internal class MenuCreatedEventTests
    {
        [Test]
        public void Creation_Name_CannotBeNull()
        {
            var creation = () => new MenuCreatedEvent(null, null!, null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_RestaurantMenuIdCannotBeNull_ThrowsException()
        {
            var creation = () => new MenuCreatedEvent(null, new Name("menu test"), null!);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_MenuIdCanBeNull_Success()
        {
            var creation = () => new MenuCreatedEvent(new MenuId(2), new Name("menu test"), null!);
            creation.Should().Throw<ArgumentNullException>();
        }
    }
}
