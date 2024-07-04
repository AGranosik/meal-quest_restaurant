using domain.Common.ValueTypes.Strings;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;

namespace unitTests.Restaurants.Aggregates.Entities
{
    [TestFixture]
    public class OwnerTests
    {
        // move id test to another tests

        private readonly Address _validAddress = new(new Street("street"), new City("city"), new Coordinates(3, 3));
        private readonly Name _validName = new("name");
        private readonly Name _validSurname = new("surname");
        private readonly OwnerId _validOwnerId = new OwnerId(1);

        [Test]
        public void Creation_OwnerIdCannotBeNull_ThrowsException()
        {
            var creation = () => Owner.Create(null, new Name("name"), new Name("sss"), _validAddress);
            creation.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Creation_NameCannotBeNull_Failed()
        {
            var result = Owner.Create(_validOwnerId, null, new Name("sss"), _validAddress);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_SurnameCannotBeNull_Success()
        {
            var result = Owner.Create(_validOwnerId, new Name("name"), null, _validAddress);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Creation_AddressCannotBeNull_Success()
        {
            var result = Owner.Create(_validOwnerId, new Name("name"), _validSurname, null);
            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void Equality_SameReference_True()
        {
            var result = Owner.Create(_validOwnerId, new Name("name"), _validSurname, _validAddress);
            (result.Value == result.Value).Should().BeTrue();
        }

        [Test]
        public void Equality_AlleTheSame_True()
        {
            var result = Owner.Create(_validOwnerId, new Name("name"), _validSurname, _validAddress);
            var result2 = Owner.Create(_validOwnerId, new Name("name"), _validSurname, _validAddress);
            (result.Value == result2.Value).Should().BeTrue();
        }

        [Test]
        public void Equality_DifferentOWnerId_False()
        {
            var result = Owner.Create(_validOwnerId, new Name("name"), _validSurname, _validAddress);
            var result2 = Owner.Create(new OwnerId(34), new Name("name"), _validSurname, _validAddress);
            (result.Value == result2.Value).Should().BeFalse();
        }
    }
}
